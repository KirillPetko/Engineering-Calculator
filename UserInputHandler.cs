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
            temp = 0;
        }

        //fields
        private int temp;
        private string result, key;
        private bool isLocked;

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
        public void addToCaption(string addition, Graphics g, FormElement textField)
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
        public void subtractFromCaption(Graphics g, FormElement textField)
        {
            if (textField.Caption != String.Empty)
            {
                temp = textField.Caption.Length - 1;
                textField.Caption = textField.Caption.Substring(0, temp);
                textField.Draw(g);
            }
        }

        //passes expression (textField.Caption) to Calculation() instance, then changes caption of 
        //text field depending on result of Calculate() function (result string or exeption message)
        //lockes if catches exeption or invalid expression message 
        public void calculateCaption(Graphics g, FormElement textField, Calculation calculation)
        {
            //in case of any non-implemented exeption display it in English
            System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
            try
            {
                calculation = new Calculation(textField.Caption);
                if (calculation.IsValid)
                {
                    result = Convert.ToString(calculation.Result);
                    result = result.Replace(",", ".");
                    textField.Caption = result;
                }
                else
                { 
                    textField.Caption = "Invalid expression";
                    isLocked = true;
                } 
            }
            catch (Exception ex) //calculation.Calculate(string input) may drop an exeption
            {                    //all exeptions are handled the same way 
                textField.Caption = ex.Message;
                isLocked = true;
            }
            textField.Draw(g);
        }

        //clears caption of textField, and unlocks itself (UserInputHandler() instance)
        public void clearCaption(Graphics g, FormElement textField)
        {
            if (textField.Caption != String.Empty)
            {
                textField.Caption = String.Empty;
                textField.Draw(g);
                isLocked = false;
            }
        }

        //handles input with pressed shift button (on-pressed-shift implementation in CalculatorForm.cs)
        public void onPressedShiftInputHandler(Graphics g, KeyEventArgs e, FormElement textField)
        {
            switch (e.KeyCode)
            {
                case Keys.D6:
                    addToCaption("^", g, textField);
                    break;
                case Keys.D8:
                    addToCaption("*", g, textField);
                    break;
                case Keys.D9:
                    addToCaption("(", g, textField);
                    break;
                case Keys.D0:
                    addToCaption(")", g, textField);
                    break;
                case Keys.Oemplus:
                    addToCaption("+", g, textField);
                    break;
                default:
                    break;
            }
        }

        //handles numpad operation keys pool
        public void numpadOperationInputHandler(Graphics g, KeyEventArgs e, FormElement textField)
        {
            switch (e.KeyCode)
            {
                case Keys.Add:
                    addToCaption("+", g, textField);
                    break;
                case Keys.Subtract:
                    addToCaption("-", g, textField);
                    break;
                case Keys.Multiply:
                    addToCaption("*", g, textField);
                    break;
                case Keys.Divide:
                    addToCaption("/", g, textField);
                    break;
                default:
                    break;
            }
        }

        //handles most input keys pressed down by calling  
        //corresponding handler functions with specific arguments
        public void majorityKeysInputHandler(Graphics g, KeyEventArgs e, FormElement textField)
        {
            bool lettersKeyPool = e.KeyCode >= Keys.A && e.KeyCode <= Keys.Z,
                 numbersKeyPool = e.KeyCode >= Keys.D0 && e.KeyCode <= Keys.D9,
                 numpadKeyPool = e.KeyCode >= Keys.NumPad0 && e.KeyCode <= Keys.NumPad9,
                 numpadOprKeyPool = e.KeyCode == Keys.Add || e.KeyCode == Keys.Subtract
                              || e.KeyCode == Keys.Multiply || e.KeyCode == Keys.Divide;
            if (e.Shift)
                onPressedShiftInputHandler(g, e, textField);
            else
            {
                if (lettersKeyPool)
                    addToCaption(e.KeyCode.ToString().ToLower(), g, textField);
                if (numpadKeyPool)
                    addToCaption(e.KeyCode.ToString().Replace("NumPad", String.Empty), g, textField);
                if (numpadOprKeyPool)
                    numpadOperationInputHandler(g, e, textField);
                if (numbersKeyPool)
                {
                    key = e.KeyCode.ToString();
                    key = key.Trim('D');
                    addToCaption(key, g, textField);
                }
                if (e.KeyCode == Keys.OemMinus)
                    addToCaption("-", g, textField);
                if (e.KeyCode == Keys.OemQuestion || e.KeyCode == Keys.Oem5)
                    addToCaption("/", g, textField);
                if (e.KeyCode == Keys.OemPeriod)
                    addToCaption(".", g, textField);
                if (e.KeyCode == Keys.Delete)
                {
                    textField.Caption = String.Empty;
                    textField.Draw(g);
                }
            }

        }

        //implements form click event by calling corresponding
        //handler functions with specific arguments
        public void buttonClickHandler(Graphics g, FormElement button, FormElement textField, Calculation calculation)
        {
            switch (button.Caption)
            {
                case "C":
                    clearCaption(g, textField);
                    break;
                case "<-":
                    subtractFromCaption(g, textField);
                    break;
                case "Pi":
                    addToCaption("3.141592", g, textField);
                    break;
                case "e":
                    addToCaption("2.71828", g, textField);
                    break;
                case "sin":
                    addToCaption("sin(", g, textField);
                    break;
                case "cos":
                    addToCaption("cos(", g, textField);
                    break;
                case "tan":
                    addToCaption("tan(", g, textField);
                    break;
                case "asin":
                    addToCaption("asin(", g, textField);
                    break;
                case "acos":
                    addToCaption("acos(", g, textField);
                    break;
                case "atan":
                    addToCaption("atan(", g, textField);
                    break;
                case "ln":
                    addToCaption("ln(", g, textField);
                    break;
                case "log":
                    addToCaption("log(", g, textField);
                    break;
                case "sqrt":
                    addToCaption("sqrt(", g, textField);
                    break;
                case "=":
                    calculateCaption(g, textField, calculation);
                    break;
                default:
                    addToCaption(button.Caption, g, textField);
                    break;
            }

        }

        //implements input keys pressed down event by calling  
        //corresponding handler functions with specific arguments
        public void keyDownHandler(Graphics g, KeyEventArgs e, FormElement textField, Calculation calculation)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    calculateCaption(g, textField, calculation);
                    break;
                case Keys.Back:
                    subtractFromCaption(g, textField);
                    break;
                default:
                    majorityKeysInputHandler(g, e, textField);
                    break;
            }
        }
    }
}
