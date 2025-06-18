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
            buffer = new Bitmap(width, height);
            FormElementsInit();
            UpdateBitmap();
            handlerUI = new UserInputHandler(formElements[0]);
        }
        //Calculation[] calculations;
        private Calculation calc;
        private UserInputHandler handlerUI;
        private FormElement[] formElements;
        private string[] captions;
        private Bitmap buffer; //faster drawing
        private int width; 
        private int height;

        public Calculation Calc
        { 
            get 
            {
                return calc;
            }
            set 
            {
                if (value == null) throw new ArgumentNullException("[Calculation] argument is null value");
                calc = value;
            }
        }
        public UserInputHandler HandlerUI
        {
            get
            {
                return handlerUI;
            }
            set
            {
                if (value == null) throw new ArgumentNullException("[HandlerUI] argument is null value");
                handlerUI = value;
            }
        }
        public FormElement[] FormElements
        {
            get
            {
                return formElements;
            }
            set
            {
                if (value == null) throw new ArgumentNullException("[FormElements] argument is null value");
                formElements = value;
            }
        }
        public string[] Captions
        {
            get
            {
                return captions;
            }
            set
            {
                if (value == null) throw new ArgumentNullException("[Captions] argument is null value");
                captions = value;
            }
        }
        public Bitmap Buffer
        {
            get
            {
                return buffer;
            }
            set
            {
                if (value == null) throw new ArgumentNullException("[Buffer] argument is null value");
                buffer = value;
            }
        }
        public int Width
        {
            get
            {
                return width;
            }
            set
            {
                width = value;
            }
        }
        public int Height
        {
            get
            {
                return height;
            }
            set
            {
                height = value;
            }
        }

        //initiallizes formElements, filling their fields
        public void FormElementsInit()
        {
            formElements = new FormElement[35];
            int bx = -25, by = 150, bWidth = 75, bHeight = 75;
            int rowButtonCounter = 0;

            for (int i = 0; i < formElements.Length; i++)
            {
                if (i == 0)
                    formElements[i] = new CustomTextField();
                else
                {
                    formElements[i] = new CustomButton(bx, by, bWidth, bHeight, captions[i - 1]);
                    rowButtonCounter++;
                }
                if (rowButtonCounter == 9)
                {
                    by += 75;
                    bx = 50;
                    rowButtonCounter = 0;
                }
                else
                    bx += 75;
            }
        }
        public void UpdateBitmap()
        {
            //clearing outdated bitmap before reinitializing
            buffer.Dispose();
            buffer = new Bitmap(width, height);
            //draw elements on bitmap
            using (Graphics graphics = Graphics.FromImage(buffer))
            {
                //increasing smoothnes of writen text
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                foreach (FormElement element in formElements)
                    element.Draw(graphics);
            }
        }
        //passes customTextFiled (formElements[0]) to 
        //UserInputHandler() instance also unlocks 
        //UserInputHandler() instance, if customTextField is cleared
        public void InvokeKeyHandler(KeyEventArgs e, Graphics g)
        {
            if (!handlerUI.IsLocked)
                handlerUI.HandleKeyDown(g, e, calc);
            if (handlerUI.IsLocked && e.KeyCode == Keys.Delete)
                handlerUI.ClearCaption(g);
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
                for (int i = 1; i < formElements.Length; i++)
                {
                    var btn = formElements[i];
                    isClicked = formElements[i].CheckPoint(e.X, e.Y);
                    buttonCaption = formElements[i].Caption;
                    if (isClicked && !handlerUI.IsLocked)
                        handlerUI.HandleButtonClick(g, btn, calc);
                    if (isClicked && buttonCaption == "C" && handlerUI.IsLocked)
                        handlerUI.ClearCaption(g);
                }
            UpdateBitmap();
        }

    }
}
