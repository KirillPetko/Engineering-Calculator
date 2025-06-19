using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Engineering_Calculator
{
    //responsible for logic, user input, and graphic data
    internal class CalculatorCore
    {
        public CalculatorCore(int _width, int _height) 
        {
            //button captions
            captions = new[]
            {
            "1","2","3","sin","cos","tan","asin","acos","atan",
            "4","5","6","ln","log","^","^2","e","Pi",
            "7","8","9","sqrt","ans","+","-","/","*",
            "0",".","(",")","C","<-","="
            };
            //supposed to be Form's w and h
            width = _width; 
            height = _height;
            Buffer = new Bitmap(width, height);
            FormElementsInit();
            UpdateBitmap();
            HandlerUI = new UserInputHandler(FormElements[0]);
        }
        //Calculation[] calculations;
        private Calculation calc;
        private UserInputHandler handlerUI;
        private FormElement[] formElements;
        private string[] captions;
        private Bitmap buffer; //faster drawing
        private int width; 
        private int height;

        internal Calculation Calc { get => calc; set => calc = value; }
        internal UserInputHandler HandlerUI { get => handlerUI; set => handlerUI = value; }
        internal FormElement[] FormElements { get => formElements; set => formElements = value; }
        public Bitmap Buffer { get => buffer; set => buffer = value; }


        //initiallizes formElements, filling their fields
        public void FormElementsInit()
        {
            FormElements = new FormElement[35];
            int bXPosition = -25, bYPosition = 150, bWidth = 75, bHeight = 75;
            int rowButtonCounter = 0;

            for (int i = 0; i < FormElements.Length; i++)
            {
                if (i == 0)
                    FormElements[i] = new CustomTextField();
                else
                {
                    FormElements[i] = new CustomButton(bXPosition, bYPosition, bWidth, bHeight, captions[i - 1]);
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
        public void UpdateBitmap()
        {
            //clearing outdated bitmap before reinitializing
            Buffer.Dispose();
            Buffer = new Bitmap(width, height);
            //draw elements on bitmap
            using (Graphics graphics = Graphics.FromImage(Buffer))
            {
                //increasing smoothnes of writen text
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                foreach (FormElement element in FormElements)
                    element.Draw(graphics);
            }
        }
        //passes customTextFiled (formElements[0]) to 
        //UserInputHandler() instance also unlocks 
        //UserInputHandler() instance, if customTextField is cleared
        public void InvokeKeyHandler(KeyEventArgs e, Graphics g)
        {
            if (!HandlerUI.IsLocked)
                HandlerUI.HandleKeyDown(g, e, Calc);
            if (HandlerUI.IsLocked && e.KeyCode == Keys.Delete)
            {
                HandlerUI.ClearCaption(g);
                HandlerUI.IsLocked = false;
            }
                
            UpdateBitmap();
        }

        //checks click location, defines clicked element, then passes  
        //customTextField (formElements[0]) to UserInputHandler()
        //also unlocks UserInputHandler() instance, if customTextField is cleared
        public void InvokeClickHandler(MouseEventArgs e, Graphics g)
        {
            bool isClicked = false;
            string buttonCaption = String.Empty;
            if (e.Button == MouseButtons.Left)
                for (int i = 1; i < FormElements.Length; i++)
                {
                    var btn = FormElements[i];
                    isClicked = FormElements[i].CheckPoint(e.X, e.Y);
                    buttonCaption = FormElements[i].Caption;
                    if (isClicked && !HandlerUI.IsLocked)
                        HandlerUI.HandleButtonClick(g, btn, Calc);
                    if (isClicked && buttonCaption == "C" && HandlerUI.IsLocked) 
                    {
                        HandlerUI.ClearCaption(g);
                        HandlerUI.IsLocked = false;
                    }
                        
                }
            UpdateBitmap();
        }

    }
}
