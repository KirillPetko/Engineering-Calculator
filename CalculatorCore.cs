using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Engineering_Calculator
{
    //responsible for logic, user input, and graphic data interactions
    //also implements initialization
    internal class CalculatorCore
    {
        public CalculatorCore(int _width, int _height, Graphics _g) 
        {
            //supposed to be Form's w and h
            width = _width; 
            height = _height;
            Buffer = new Bitmap(width, height);
            g = _g;

            Containers = new ContainerManager();
            UpdateBitmap();

            exHandler = new ExceptionHandler();
            HandlerUI = new UserInputHandler(Containers.GetCustomTextField(), exHandler, g);
        }

        //fields
        Graphics g;
        private UserInputHandler handlerUI;
        private ContainerManager сontainers;
        private Bitmap buffer; //implemented for faster drawing
        private readonly ExceptionHandler exHandler;
        

        private int width; 
        private int height;

        //internal Calculation Calc { get => emptyCalculation; set => emptyCalculation = value; }
        internal UserInputHandler HandlerUI { get => handlerUI; set => handlerUI = value; }
        public Bitmap Buffer { get => buffer; set => buffer = value; }
        public ExceptionHandler ExHandler => exHandler;
        internal ContainerManager Containers { get => сontainers; set => сontainers = value; }

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
                //foreach (FormElement element in Containers.ElementsUI)
                //    element.Draw(graphics);
                for (int i = 0;i< Containers.ElementsUI.Length;i++)
                    Containers.ElementsUI[i].Draw(graphics);
            }
        }
        //passes key data to UserInputHandler() instance
        //to handle KeyDown() event. Also unlocks
        //UserInputHandler(), if customTextField is cleared
        public void InvokeKeyHandler(KeyEventArgs e)
        {
            if (!HandlerUI.IsLocked)
            {
                HandlerUI.HandleKeyDown(e);
                if (e.KeyCode == Keys.Enter && HandlerUI.IsValidProduct())
                { 
                    Containers.Calculations.Add(HandlerUI.Product);
                    Containers.SaveLastCalculationToTextFile();
                    HandlerUI.Product = null;
                }   
            }
            if (HandlerUI.IsLocked && e.KeyCode == Keys.Delete)
            {
                HandlerUI.ClearCaption();
                HandlerUI.IsLocked = false;
            }
            UpdateBitmap();
        }

        //checks click location, defines clicked element, by 
        //calling FormElement:CheckPoint() method. Also unlocks
        //UserInputHandler() instance, if customTextField is cleared
        public void InvokeClickHandler(MouseEventArgs e)
        {
            bool isClicked = false;
            string buttonCaption = String.Empty;
            if (e.Button == MouseButtons.Left)
                for (int i = 1; i < Containers.ElementsUI.Length; i++)
                {
                    var btn = Containers.ElementsUI[i];
                    isClicked = btn.CheckPoint(e.X, e.Y);
                    buttonCaption = btn.Caption;
                    if (isClicked && !HandlerUI.IsLocked)
                    { 
                        HandlerUI.HandleButtonClick(btn);
                        if (buttonCaption == "=" && HandlerUI.IsValidProduct())
                        { 
                            Containers.Calculations.Add(HandlerUI.Product);
                            Containers.SaveLastCalculationToTextFile();
                            HandlerUI.Product = null;
                        }    
                    }  
                    if (isClicked && buttonCaption == "C" && HandlerUI.IsLocked) 
                    {
                        HandlerUI.ClearCaption();
                        HandlerUI.IsLocked = false;
                    }  
                }
            UpdateBitmap();
        }

    }
}
