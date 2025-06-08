using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Engineering_Calculator
{
    public partial class Form1 : Form
    {
       
        public Form1()
        {
            InitializeComponent();
            //first button coordds (bx = -25 because first addittion slides button)
            int bx = -25, by = 150, bWidth = 75, bHeight = 75;
            //counter used for aligning 9 buttons in one row
            int rowButtonCounter = 0;
            for (int i = 0; i < formElements.Length; i++)
            {
                //formElements[i] = i != 0 ? new customButton() : new customTextField();//9.0

                if (i == 0)
                    formElements[i] = new customTextField();
                else
                {
                    //since one of formElements is a customTextfield number of captions is less than elements by 1
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
                {
                    bx += 75;
                   
                }

            }
                

        }
        //fields
        private FormElement[] formElements = new FormElement[35];
        string[] captions = {
                             "1","2","3","sin","cos","tan","asin","acos","atan",
                             "4","5","6","ln","log","^","^2","e","Pi",
                             "7","8","9","sqrt","ans","+","-","/","*",
                             "0","C",".","(",")","<<<","="
                             };
        private void Form1_Load(object sender, System.EventArgs e)
        {
          
        }

        //creating visual representation of formElements on Paint event
        private void Form1_Paint(object sender, PaintEventArgs e) 
        {
            foreach (FormElement element in formElements)
                element.Draw(e.Graphics);
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            //to properly give a handler for graphics to drawing function graphics has to be crated in mouse handler function
            System.Drawing.Graphics g = this.CreateGraphics();

            if (e.Button == MouseButtons.Left)
                for (int i = 1; i < formElements.Length; i++)
                {
                    if (formElements[i].checkPoint(e.X, e.Y))
                        {
                            switch (formElements[i].Caption)
                            {
                                case "C":
                                    formElements[0].Caption = String.Empty;
                                    formElements[0].Draw(g);
                                    break;
                                case "<<<":
                                    formElements[0].Caption = formElements[0].Caption.Substring(0, formElements[0].Caption.Length - 1);
                                    formElements[0].Draw(g);
                                    break;
                                case "Pi":
                                    formElements[0].Caption = "3.141592";
                                    formElements[0].Draw(g);
                                    break;
                                case "e":
                                    formElements[0].Caption = "2.71828";
                                    formElements[0].Draw(g);
                                    break;
                                case "sin":
                                    formElements[0].Caption += formElements[i].Caption + '(';
                                    formElements[0].Draw(g);
                                    break;
                                case "cos":
                                    formElements[0].Caption += formElements[i].Caption + '(';
                                    formElements[0].Draw(g);
                                    break;
                                case "tan":
                                    formElements[0].Caption += formElements[i].Caption + '(';
                                    formElements[0].Draw(g);
                                    break;
                                case "asin":
                                    formElements[0].Caption += formElements[i].Caption + '(';
                                    formElements[0].Draw(g);
                                    break;
                                case "acos":
                                    formElements[0].Caption += formElements[i].Caption + '(';
                                    formElements[0].Draw(g);
                                    break;
                                case "atan":
                                    formElements[0].Caption += formElements[i].Caption + '(';
                                    formElements[0].Draw(g);
                                    break;
                                case "ln":
                                    formElements[0].Caption += formElements[i].Caption + '(';
                                    formElements[0].Draw(g);
                                    break;
                                case "log":
                                    formElements[0].Caption += formElements[i].Caption + '(';
                                    formElements[0].Draw(g);
                                    break;
                                case "=":
                                    try
                                    {
                                        Calculation calculation = new Calculation(formElements[0].Caption);
                                        if (calculation.IsValid) formElements[0].Caption = Convert.ToString(calculation.Result);
                                        else formElements[0].Caption = "Invalid expression";
                                    }
                                    catch (Exception ex)  // calculation.Calculate(string input) may drop an exeption
                                    {
                                        formElements[0].Caption = ex.Message;
                                    }
                                    formElements[0].Draw(g);
                                    break;
                                default:
                                    formElements[0].Caption += formElements[i].Caption;
                                    formElements[0].Draw(g);
                                    break;


                            }                  
                        }
                    }             
        }
    }
}
