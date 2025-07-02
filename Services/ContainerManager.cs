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
        public ContainerManager(IFormElementFactory _factory)
        {
            buttonCaptions = new[]
            {
            "1","2","3","sin","cos","tan","asin","acos","atan",
            "4","5","6","ln","log","^","^2","e","Pi",
            "7","8","9","sqrt","ans","+","-","/","*",
            "0",".","(",")","C","<-","="
            };
            calculations = new List<Calculation> { };
            Factory = _factory;
            FormElementsInit();
        }
        private FormElement[] elementsUI;
        private string[] buttonCaptions;
        private readonly List<Calculation> calculations;
        private IFormElementFactory factory;

        internal FormElement[] ElementsUI { get => elementsUI; set => elementsUI = value; }
        internal List<Calculation> Calculations => calculations;
        public IFormElementFactory Factory { get => factory; set => factory = value; }

        //methods

        //initiallizes formElements, filling their fields
        public void FormElementsInit()
        {
            ElementsUI = new FormElement[36];
            int bXPosition = 50, bYPosition = 150;
            int rowButtonCounter = 0;

            for (int i = 0; i < ElementsUI.Length; i++)
            {
                if (i == 0)
                    ElementsUI[i] = Factory.CreateTextField();
                else if (i == 1)
                    ElementsUI[i] = Factory.CreateHistoryButton();
                else
                {
                    ElementsUI[i] = Factory.CreateButton(bXPosition, bYPosition, buttonCaptions[i - 2]);
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

        public CustomTextField GetCustomTextField()
        {
            FormElement textField = ElementsUI.First(elem => elem.GetTypeString() == "CustomTextField");
            return textField as CustomTextField;
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
