using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Engineering_Calculator
{
    internal class UserInputHandler
    {
        public UserInputHandler() 
        {
            result = String.Empty;
            key = String.Empty;
            isLocked = false;
            temp = 0;

            exHandler = new ExceptionHandler();
            exHandler.AddObserver(new ErrorLogger());
            exHandler.AddObserver(new UserNotification());
        }

        public UserInputHandler(FormElement _textField)
        {
            result = String.Empty;
            key = String.Empty;
            isLocked = false;
            temp = 0;
            textField = _textField;

            exHandler = new ExceptionHandler();
            exHandler.AddObserver(new ErrorLogger());
            exHandler.AddObserver(new UserNotification(textField));
        }


        //fields
        private int temp;
        private string result, key;
        private bool isLocked;
        private FormElement textField;
        private readonly ExceptionHandler exHandler;

        public bool IsLocked
        {
            get
            {
                return isLocked;
            }
            set
            {
                isLocked = value;
            }
        }

        //methods

        //adds string to caption
        public void AddToCaption(string addition, Graphics g)
        {
            if (textField != null)
            {
                textField.Caption += addition;
                textField.Draw(g);
            }
            else
                throw new NotImplementedException("custom text field was not initialized");
        }

        //subtracts one symbol from textField's caption
        public void SubtractFromCaption(Graphics g)
        {
            if (textField.Caption != String.Empty)
            {
                temp = textField.Caption.Length - 1;
                textField.Caption = textField.Caption.Substring(0, temp);
                textField.Draw(g);
            }
        }

        //changes caption of text field depending on result of Calculate() 
        //function (result string or exeption message), lockes if catches 
        //exeption or invalid expression message, displaying related message 
        public void CalculateCaption(Graphics g, Calculation calc)
        {
            //in case of any non-implemented exeption display it in English
            System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
            try
            {
                calc = new Calculation(textField.Caption);
                if (calc.IsValid)
                {
                    result = Convert.ToString(calc.Result);
                    result = result.Replace(",", ".");
                    textField.Caption = result;
                }
                else
                { 
                    textField.Caption = "Invalid input";
                    isLocked = true;
                } 
            }
            catch (Exception ex) 
            {   
                //textField.Caption = ex.Message;
                exHandler.HandleException(ex);
                isLocked = true;
            }
            textField.Draw(g);
        }

        //clears caption of textField, and unlocks itself (UserInputHandler() instance)
        public void ClearCaption(Graphics g)
        {
            if (textField.Caption != String.Empty)
            {
                textField.Caption = String.Empty;
                textField.Draw(g);
                isLocked = false;
            }
        }

        //handles input with pressed shift button (implementation in CalculatorCore.cs)
        public void HandleShiftPressed(Graphics g, KeyEventArgs e, FormElement textField)
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
            if (e.Shift)
                HandleShiftPressed(g, e, textField);
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
                if (e.KeyCode == Keys.Delete)
                {
                    textField.Caption = String.Empty;
                    textField.Draw(g);
                }
            }
        }

        //implements form click event by calling corresponding
        //handler functions with specific arguments
        public void HandleButtonClick(Graphics g, FormElement button,  Calculation calc)
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
                case "=":
                    CalculateCaption(g, calc);
                    break;
                default:
                    AddToCaption(button.Caption, g);
                    break;
            }
        }

        //implements expression keys pressed down event by calling  
        //corresponding handler functions with specific arguments
        public void HandleKeyDown(Graphics g, KeyEventArgs e, Calculation calc)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    CalculateCaption(g, calc);
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
