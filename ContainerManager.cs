using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace Engineering_Calculator
{
    internal class ContainerManager
    {
        //manages core-related containers FormElement[] and List<Calculation>
        public ContainerManager()
        {
            buttonCaptions = new[]
            {
            "1","2","3","sin","cos","tan","asin","acos","atan",
            "4","5","6","ln","log","^","^2","e","Pi",
            "7","8","9","sqrt","ans","+","-","/","*",
            "0",".","(",")","C","<-","="
            };
            FormElementsInit();
            calculations = new List<Calculation> { };
        }
        private FormElement[] elementsUI;
        private string[] buttonCaptions;
        private readonly List<Calculation> calculations;

        internal FormElement[] ElementsUI { get => elementsUI; set => elementsUI = value; }
        internal List<Calculation> Calculations => calculations;

        //methods

        //initiallizes formElements, filling their fields
        public void FormElementsInit()
        {
            ElementsUI = new FormElement[36];
            int bXPosition = 50, bYPosition = 150, bWidth = 75, bHeight = 75;
            int rowButtonCounter = 0;

            for (int i = 0; i < ElementsUI.Length; i++)
            {
                if (i == 0)
                    ElementsUI[i] = new CustomTextField();
                else if(i==1)
                    ElementsUI[i] = new HistoryButton();
                else
                {
                    ElementsUI[i] = new CustomButton(bXPosition, bYPosition, bWidth, bHeight, buttonCaptions[i - 2]);
                    bXPosition += 75;
                    rowButtonCounter++;
                }
                if (rowButtonCounter == 9)
                {
                    bYPosition += 75;
                    bXPosition = 50;
                    rowButtonCounter = 0;
                }      
            }
            var textFiled = GetCustomTextField();
            textFiled.HButtonToDraw = ElementsUI[1];
        }

        //returns user input field
        public CustomTextField GetCustomTextField()
        {
            CustomTextField textField = null;
            for (int i = 0; i < ElementsUI.Length; i++)
                if (ElementsUI[i].GetTypeString() == "CustomTextField")
                { 
                    textField = ElementsUI[i] as CustomTextField;
                    break;
                }
            if (textField == null) 
                throw new ArgumentNullException("failed to initiallize FormElement:CustomTextField");

            return textField;
        }

        public void SaveLastCalculationToTextFile()
        {
            string expToSave = String.Empty,
                   lExpression = String.Empty,
                   lResult = String.Empty;
            Calculation last = Calculations.Last();
            if (last != null)
            {
                lExpression = last.Expression;
                lResult = last.Result.ToString().Replace(",", ".");
                if (lExpression != lResult)
                { 
                    expToSave += DateTime.Now + "\t";
                    expToSave += lExpression;
                    expToSave += "  =  ";
                    expToSave += lResult;
                }
            }
            if(!String.IsNullOrEmpty(expToSave))
                FileManager.RecordCalculation(expToSave);
        }
    }
}
