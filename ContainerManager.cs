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
            fM = new FileManager();
        }
        private FormElement[] elementsUI;
        private string[] buttonCaptions;
        private readonly List<Calculation> calculations;
        private readonly FileManager fM;

        internal FormElement[] ElementsUI { get => elementsUI; set => elementsUI = value; }
        internal List<Calculation> Calculations => calculations;

        internal FileManager FM => fM;


        //methods

        //initiallizes formElements, filling their fields
        public void FormElementsInit()
        {
            ElementsUI = new FormElement[35];
            int bXPosition = -25, bYPosition = 150, bWidth = 75, bHeight = 75;
            int rowButtonCounter = 0;

            for (int i = 0; i < ElementsUI.Length; i++)
            {
                if (i == 0)
                    ElementsUI[i] = new CustomTextField();
                else
                {
                    ElementsUI[i] = new CustomButton(bXPosition, bYPosition, bWidth, bHeight, buttonCaptions[i - 1]);
                    rowButtonCounter++;
                }
                if (rowButtonCounter == 9)
                {
                    bYPosition += 75;
                    bXPosition = 50;
                    rowButtonCounter = 0;
                }
                else
                    bXPosition += 75;
            }
        }

        //returns user input field
        public FormElement GetCustomTextField()
        {
            FormElement textField = null;
            for (int i = 0; i < ElementsUI.Length; i++)
                if (ElementsUI[i].GetTypeString() == "CustomTextField")
                { 
                    textField = ElementsUI[i];
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
                fM.RecordCalculation(expToSave);
        }
    }
}
