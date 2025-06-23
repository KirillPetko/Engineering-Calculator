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
        public CalculatorCore(int _width, int _height) 
        {
            //supposed to be Form's w and h
            width = _width; 
            height = _height;
            Buffer = new Bitmap(width, height);

            Containers = new ContainerManager();
            UpdateBitmap();

            errFactory = new ErrorFactory();
            exHandler = new ExceptionHandler();
            HandlerUI = new UserInputHandler(Containers.GetCustomTextField(), ErrFactory, exHandler);
        }

        //fields
        private UserInputHandler handlerUI;
        private ContainerManager сontainers;
        private Bitmap buffer; //implemented for faster drawing

        private readonly ErrorFactory errFactory;
        private readonly ExceptionHandler exHandler;

        private int width; 
        private int height;

        //internal Calculation Calc { get => emptyCalculation; set => emptyCalculation = value; }
        internal UserInputHandler HandlerUI { get => handlerUI; set => handlerUI = value; }
        public Bitmap Buffer { get => buffer; set => buffer = value; }
        public ErrorFactory ErrFactory { get => errFactory; }
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
                foreach (FormElement element in Containers.ElementsUI)
                    element.Draw(graphics);
            }
        }
        //passes key data to UserInputHandler() instance
        //to handle KeyDown() event. Also unlocks
        //UserInputHandler(), if customTextField is cleared
        public void InvokeKeyHandler(KeyEventArgs e, Graphics g)
        {
            if (!HandlerUI.IsLocked)
            {
                HandlerUI.HandleKeyDown(g, e);
                if (e.KeyCode == Keys.Enter)
                { 
                    Containers.Calculations.Add(HandlerUI.Product);
                    Containers.SaveLastCalculationToTextFile();
                }
                    
            }
                
            if (HandlerUI.IsLocked && e.KeyCode == Keys.Delete)
            {
                HandlerUI.ClearCaption(g);
                HandlerUI.IsLocked = false;
            }

            UpdateBitmap();
        }

        //checks click location, defines clicked element, by 
        //calling FormElement:CheckPoint() method. Also unlocks
        //UserInputHandler() instance, if customTextField is cleared
        public void InvokeClickHandler(MouseEventArgs e, Graphics g)
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
                        HandlerUI.HandleButtonClick(g, btn);
                        if (buttonCaption == "=" && HandlerUI.Product != null) 
                            Containers.Calculations.Add(HandlerUI.Product);
                    }  
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
