using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Engineering_Calculator
{
    //produces reaction on key press and button click events 
    //in form of redrawing or Calculation() instance 
    internal class UserInputHandler
    {
        public UserInputHandler(CustomTextField _textFiled, ErrorFactory _errFactory, ExceptionHandler _exHandler)
        {
            result = String.Empty;
            key = String.Empty;
            lastAnswer = "0";
            IsLocked = false;
            textField = _textFiled;
 
            errFactory = _errFactory;
            exHandler = _exHandler;
            product = null;
            exHandler.AddObserver(new ErrorLogger());
            exHandler.AddObserver(new UserNotification(textField));
        }

        //fields
        private string result, key, lastAnswer;
        private bool isLocked;
        private CustomTextField textField;
        private readonly ErrorFactory errFactory;
        private readonly ExceptionHandler exHandler;
        private Calculation product;

        public bool IsLocked { get => isLocked; set => isLocked = value; }
        public ExceptionHandler ExHandler => exHandler;

        internal ErrorFactory ErrFactory { get => errFactory;}
        internal Calculation Product { get => product; set => product = value; }

        //methods

        //adds string to inputCaption
        public void AddToCaption(string addition, Graphics g)
        {
            if (textField != null)
            {
                textField.Caption += addition;
                textField.Draw(g);
            }
        }

        //subtracts one symbol from textField's inputCaption
        public void SubtractFromCaption(Graphics g)
        {
            if (textField.Caption != String.Empty)
            {
                textField.Caption = textField.Caption.Substring(0, textField.Caption.Length - 1);
                textField.Draw(g);
            }
        }

        //changes inputCaption of text field depending on result of Calculate() 
        //function (result string or exeption message), lockes if catches 
        //exeption or invalid expression message, displaying related message 
        public void CalculateCaption(Graphics g)
        {
            //in case of any non-implemented exeption display it in English
            System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
            try
            {
                Product = new Calculation(textField.Caption, ErrFactory);
                result = Convert.ToString(Product.Result);
                textField.Caption = result.Replace(",", ".");
                lastAnswer = textField.Caption;
                textField.UpperCaption = "ans = " + lastAnswer;
            }
            catch (Exception ex) 
            {
                ExHandler.HandleException(ex);
                IsLocked = true;
                textField.UpperCaption = String.Empty;
            }
            textField.Draw(g);
        }

        //clears inputCaption of textField, and unlocks itself (UserInputHandler() instance)
        public void ClearCaption(Graphics g)
        {
                textField.Caption = String.Empty;
                textField.UpperCaption = String.Empty;
                textField.Draw(g);
        }

        //handles input with pressed shift button (implementation in CalculatorCore.cs)
        public void HandleShiftPressed(Graphics g, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.D6:
                    AddToCaption("^", g);
                    break;
                case Keys.D8:
                    AddToCaption("*", g);
                    break;
                case Keys.D9:
                    AddToCaption("(", g);
                    break;
                case Keys.D0:
                    AddToCaption(")", g);
                    break;
                case Keys.Oemplus:
                    AddToCaption("+", g);
                    break;
                default:
                    break;
            }
        }

        //implements copy-paste function
        public void HandleControlPressed(Graphics g, KeyEventArgs e) 
        {
            switch (e.KeyCode)
            {
                case Keys.C:
                    Clipboard.SetText(textField.Caption);
                    break;
                case Keys.V:
                    string textToPaste = Clipboard.GetText();
                    textToPaste = Regex.Replace(textToPaste, @"\s+", String.Empty);
                    AddToCaption(textToPaste, g);
                    break;
                default :
                    break;
            }
        }

        //handles numpad operation keys pool
        public void HandleNumpadOperations(Graphics g, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Add:
                    AddToCaption("+", g);
                    break;
                case Keys.Subtract:
                    AddToCaption("-", g);
                    break;
                case Keys.Multiply:
                    AddToCaption("*", g);
                    break;
                case Keys.Divide:
                    AddToCaption("/", g);
                    break;
                default:
                    break;
            }
        }

        //handles most keys pressed down by calling  
        //corresponding functions with specific arguments
        public void HandleMajorityKeys(Graphics g, KeyEventArgs e)
        {
            bool lettersKeyPool = e.KeyCode >= Keys.A && e.KeyCode <= Keys.Z,
                 numbersKeyPool = e.KeyCode >= Keys.D0 && e.KeyCode <= Keys.D9,
                 numpadKeyPool = e.KeyCode >= Keys.NumPad0 && e.KeyCode <= Keys.NumPad9,
                 numpadOprKeyPool = e.KeyCode == Keys.Add || e.KeyCode == Keys.Subtract
                              || e.KeyCode == Keys.Multiply || e.KeyCode == Keys.Divide;
            if (e.Control) 
                HandleControlPressed(g, e);
            else if (e.Shift)
                HandleShiftPressed(g, e);
            else
            {
                if (lettersKeyPool)
                    AddToCaption(e.KeyCode.ToString().ToLower(), g);
                if (numpadKeyPool)
                    AddToCaption(e.KeyCode.ToString().Replace("NumPad", String.Empty), g);
                if (numpadOprKeyPool)
                    HandleNumpadOperations(g, e);
                if (numbersKeyPool)
                {
                    key = e.KeyCode.ToString();
                    key = key.Trim('D');
                    AddToCaption(key, g);
                }
                if (e.KeyCode == Keys.OemMinus)
                    AddToCaption("-", g);
                if (e.KeyCode == Keys.OemQuestion || e.KeyCode == Keys.Oem5)
                    AddToCaption("/", g);
                if (e.KeyCode == Keys.OemPeriod)
                    AddToCaption(".", g);
                if (e.KeyCode == Keys.Oemcomma)
                    AddToCaption(".", g);
                if (e.KeyCode == Keys.Delete)
                    ClearCaption(g);
            }
        }

        //implements form click event by calling corresponding
        //handler functions with specific arguments
        public void HandleButtonClick(Graphics g, FormElement button)
        {
            switch (button.Caption)
            {
                case "C":
                    ClearCaption(g);
                    break;
                case "<-":
                    SubtractFromCaption(g);
                    break;
                case "Pi":
                    AddToCaption("3.141592", g);
                    break;
                case "e":
                    AddToCaption("2.71828", g);
                    break;
                case "sin":
                    AddToCaption("sin(", g);
                    break;
                case "cos":
                    AddToCaption("cos(", g);
                    break;
                case "tan":
                    AddToCaption("tan(", g);
                    break;
                case "asin":
                    AddToCaption("asin(", g);
                    break;
                case "acos":
                    AddToCaption("acos(", g);
                    break;
                case "atan":
                    AddToCaption("atan(", g);
                    break;
                case "ln":
                    AddToCaption("ln(", g);
                    break;
                case "log":
                    AddToCaption("log(", g);
                    break;
                case "sqrt":
                    AddToCaption("sqrt(", g);
                    break;
                case "ans":
                    AddToCaption(lastAnswer, g);
                    break;
                case "=":
                    product = new Calculation();
                    CalculateCaption(g);
                    break;
                case "HistoryButton":
                    System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
                    try { System.Diagnostics.Process.Start("history.txt"); }
                    catch (Exception ex)
                    {
                        ExHandler.HandleException(ex);
                        IsLocked = true;
                        textField.UpperCaption = String.Empty;
                    }
                    break;
                default:
                    AddToCaption(button.Caption, g);
                    break;
            }
        }

        //implements expression keys pressed down event by calling  
        //corresponding handler functions with specific arguments
        public void HandleKeyDown(Graphics g, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    CalculateCaption(g);
                    break;
                case Keys.Back:
                    SubtractFromCaption(g);
                    break;
                default:
                    HandleMajorityKeys(g, e);
                    break;
            }
        }

    }
}
