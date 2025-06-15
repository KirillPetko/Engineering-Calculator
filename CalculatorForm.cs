using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Engineering_Calculator
{
    public partial class CalculatorForm : Form
    {
        public CalculatorForm()
        {
            InitializeComponent();
            //first button coordds (bx = -25 because first addittion slides button)
            int bx = -25, by = 150, bWidth = 75, bHeight = 75;
            //counter used for aligning 9 buttons in one row
            int rowButtonCounter = 0;

            for (int i = 0; i < formElements.Length; i++)
            {
                if (i == 0)
                    formElements[i] = new customTextField();
                else
                {
                    //since one of formElements is a customTextfield 
                    //number of captions is less than elements by 1
                    formElements[i] = new customButton(bx, by, bWidth, bHeight, captions[i-1]);
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

        //fields
        Calculation calculation;
        System.Drawing.Graphics g;
        UserInputHandler handlerUI = new UserInputHandler();

        private FormElement[] formElements = new FormElement[35];
        private string[] captions = {
                             "1","2","3","sin","cos","tan","asin","acos","atan",
                             "4","5","6","ln","log","^","^2","e","Pi",
                             "7","8","9","sqrt","ans","+","-","/","*",
                             "0",".","(",")","C","<-","="
                             };
        
        //methods
        private void CalculatorForm_Load(object sender, EventArgs e)
        {

        }

        //creating visual representation of formElements on Paint event
        private void CalculatorForm_Paint(object sender, PaintEventArgs e)
        {
            foreach (FormElement element in formElements)
                element.Draw(e.Graphics);
        }

        //responsible for key pressing event: passes customTextFiled 
        //(formElements[0]) to UserInputHandler() instance also unlocks 
        //UserInputHandler() instance, if customTextField is cleared
        private void CalculatorForm_KeyDown(object sender, KeyEventArgs e)
        { 
            //graphics has to be created to give a control to drawing function
            g = this.CreateGraphics();
            
                if (!handlerUI.IsLocked)
                    handlerUI.keyDownHandler(g, e, formElements[0], calculation);
                if(handlerUI.IsLocked && e.KeyCode == Keys.Delete)
                    handlerUI.clearCaption(g, formElements[0]);
        }

        //responsible for mouse click event: checks click location, defines clicked  
        //element, then passes customTextField (formElements[0]) to UserInputHandler()
        //also unlocks UserInputHandler() instance, if customTextField is cleared
        private void CalculatorForm_MouseClick(object sender, MouseEventArgs e)
        {
            g = this.CreateGraphics();
            bool isClicked = false;
            string buttonCaption = String.Empty;

            if (e.Button == MouseButtons.Left)
                for (int i = 1; i < formElements.Length; i++)
                {
                    isClicked = formElements[i].checkPoint(e.X, e.Y);
                    buttonCaption = formElements[i].Caption;
                    if (isClicked && !handlerUI.IsLocked)
                        handlerUI.buttonClickHandler(g, formElements[i], formElements[0], calculation);
                    if (isClicked && buttonCaption == "C" && handlerUI.IsLocked)
                        handlerUI.clearCaption(g, formElements[0]);
                }
        }
    }
}
