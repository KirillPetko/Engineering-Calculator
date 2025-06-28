using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public UserInputHandler(CustomTextField _textFiled, ExceptionHandler _exHandler, Graphics _g)
        {
            result = String.Empty;
            key = String.Empty;
            lastAnswer = "0";
            IsLocked = false;
            textField = _textFiled;

            exHandler = _exHandler;
            exHandler.AddObserver(new ErrorLogger());
            exHandler.AddObserver(new UserNotification(textField));
            g = _g;
        }

        //fields
        private string result, key, lastAnswer;
        private bool isLocked;
        private Graphics g;
        private CustomTextField textField;
        private readonly ExceptionHandler exHandler;
        private Calculation product;
        public event Action HistoryRequested;

        public bool IsLocked { get => isLocked; set => isLocked = value; }
        public ExceptionHandler ExHandler => exHandler;
        internal Calculation Product { get => product; set => product = value; }
        
        //methods

        //adds string to inputCaption
        public void AddToCaption(string addition)
        {
            if (textField != null)
            {
                textField.Caption += addition;
                textField.Draw(g);
            }
        }

        //subtracts one symbol from textField's inputCaption
        public void SubtractFromCaption()
        {
            if (textField.Caption != String.Empty)
            {
                textField.Caption = textField.Caption.Substring(0, textField.Caption.Length - 1);
                textField.Draw(g);
            }
        }

        //changes inputCaption of text field depending on result of Calculate() 
        //function (result string or exeption errorMsg), lockes if catches 
        //exeption or invalid expression errorMsg, displaying related errorMsg 
        public void CalculateCaption()
        {
            product = new Calculation();
            //in case of any non-implemented exeption display it in English
            System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
            try
            {
                if(ExecuteCommand())
                    return;
                Product = new Calculation(textField.Caption);
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
        public void ClearCaption()
        {
            textField.Caption = String.Empty;
            textField.UpperCaption = String.Empty;
            textField.Draw(g);
        }

        //handles input with pressed shift button (implementation in CalculatorCore.cs)
        public void HandleShiftPressed(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.D6:
                    AddToCaption("^");
                    break;
                case Keys.D8:
                    AddToCaption("*");
                    break;
                case Keys.D9:
                    AddToCaption("(");
                    break;
                case Keys.D0:
                    AddToCaption(")");
                    break;
                case Keys.Oemplus:
                    AddToCaption("+");
                    break;
                default:
                    break;
            }
        }

        //implements copy-paste function
        public void HandleControlPressed(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.C:
                    Clipboard.SetText(textField.Caption);
                    break;
                case Keys.V:
                    string textToPaste = Clipboard.GetText();
                    textToPaste = Regex.Replace(textToPaste, @"\s+", String.Empty);
                    AddToCaption(textToPaste);
                    break;
                default:
                    break;
            }
        }

        //handles numpad operation keys pool
        public void HandleNumpadOperations(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Add:
                    AddToCaption("+");
                    break;
                case Keys.Subtract:
                    AddToCaption("-");
                    break;
                case Keys.Multiply:
                    AddToCaption("*");
                    break;
                case Keys.Divide:
                    AddToCaption("/");
                    break;
                default:
                    break;
            }
        }

        //handles most keys pressed down by calling  
        //corresponding functions with specific arguments
        public void HandleMajorityKeys(KeyEventArgs e)
        {
            bool lettersKeyPool = e.KeyCode >= Keys.A && e.KeyCode <= Keys.Z,
                 numbersKeyPool = e.KeyCode >= Keys.D0 && e.KeyCode <= Keys.D9,
                 numpadKeyPool = e.KeyCode >= Keys.NumPad0 && e.KeyCode <= Keys.NumPad9,
                 numpadOprKeyPool = e.KeyCode == Keys.Add || e.KeyCode == Keys.Subtract
                              || e.KeyCode == Keys.Multiply || e.KeyCode == Keys.Divide;
            if (e.Control)
                HandleControlPressed(e);
            else if (e.Shift)
                HandleShiftPressed(e);
            else
            {
                if (lettersKeyPool)
                    AddToCaption(e.KeyCode.ToString().ToLower());
                if (numpadKeyPool)
                    AddToCaption(e.KeyCode.ToString().Replace("NumPad", String.Empty));
                if (numpadOprKeyPool)
                    HandleNumpadOperations(e);
                if (numbersKeyPool)
                {
                    key = e.KeyCode.ToString();
                    key = key.Trim('D');
                    AddToCaption(key);
                }
                if (e.KeyCode == Keys.OemMinus)
                    AddToCaption("-");
                if (e.KeyCode == Keys.OemQuestion || e.KeyCode == Keys.Oem5)
                    AddToCaption("/");
                if (e.KeyCode == Keys.OemPeriod)
                    AddToCaption(".");
                if (e.KeyCode == Keys.Oemcomma)
                    AddToCaption(".");
                if (e.KeyCode == Keys.Delete)
                    ClearCaption();
            }
        }

        //implements form click event by calling corresponding
        //handler functions with specific arguments
        public void HandleButtonClick(FormElement button)
        {
            switch (button.Caption)
            {
                case "C":
                    ClearCaption();
                    break;
                case "<-":
                    SubtractFromCaption();
                    break;
                case "Pi":
                    AddToCaption("3.141592");
                    break;
                case "e":
                    AddToCaption("2.71828");
                    break;
                case "sin":
                    AddToCaption("sin(");
                    break;
                case "cos":
                    AddToCaption("cos(");
                    break;
                case "tan":
                    AddToCaption("tan(");
                    break;
                case "asin":
                    AddToCaption("asin(");
                    break;
                case "acos":
                    AddToCaption("acos(");
                    break;
                case "atan":
                    AddToCaption("atan(");
                    break;
                case "ln":
                    AddToCaption("ln(");
                    break;
                case "log":
                    AddToCaption("log(");
                    break;
                case "sqrt":
                    AddToCaption("sqrt(");
                    break;
                case "ans":
                    AddToCaption(lastAnswer);
                    break;
                case "=":
                    CalculateCaption();
                    break;
                case "HistoryButton":
                    HistoryRequested.Invoke();
                    break;
                default:
                    AddToCaption(button.Caption);
                    break;
            }
        }

        //implements expression keys pressed down event by calling  
        //corresponding handler functions with specific arguments
        public void HandleKeyDown(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    CalculateCaption();
                    break;
                case Keys.Back:
                    SubtractFromCaption();
                    break;
                default:
                    HandleMajorityKeys(e);
                    break;
            }
        }

        public bool ExecuteCommand()
        {
            switch (textField.Caption)
            {
                case "-history":
                    FileManager.OpenFile(textField.Caption);
                    ClearCaption();
                    return true;
                case "-log":
                    FileManager.OpenFile(textField.Caption);
                    ClearCaption();
                    return true;
                default:
                    return false;
            }
        }
        public bool IsValidProduct()
        {
            if (product.Input!=Convert.ToString(product.Result) && product.IsValidExpression)
                return true;
            return false;
        }
    }
}
