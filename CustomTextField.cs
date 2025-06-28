using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using System.Text.RegularExpressions;

namespace Engineering_Calculator
{
    //class responsible for graphical representation of a custom text field
    internal class CustomTextField:FormElement
    {
        public CustomTextField() 
        {
            X = 50;
            Y = 30;
            Width = 675; 
            Height = 80;
            Caption = String.Empty;
            UpperCaption = String.Empty;    

            rect = new Rectangle(x, y, width, height);
            backgroundBrush = new SolidBrush(Color.Black);
            inputBrush = new SolidBrush(Color.Orange);
            upperBrush = new SolidBrush(Color.LimeGreen);
            inputFont = new Font("Consolas", 30);//even character width and height
            upperFont = new Font("Consolas", 15);
        }

        private int x;
        private int y;
        private int width;
        private int height;
        private string inputCaption;
        private string upperCaption;
        private Rectangle rect;
        private Brush backgroundBrush, inputBrush, upperBrush;
        private Font inputFont, upperFont;
        private FormElement hButtonToDraw;

        public override int X { get => x; set => x = value; }
        public override int Y { get => y; set => y = value; }
        public override int Width { get => width; set => width = value; }
        public override int Height { get => height; set => height = value; }
        public override string Caption { get => inputCaption; set => inputCaption = value; }
        public string UpperCaption { get => upperCaption; set => upperCaption = value; }
        internal FormElement HButtonToDraw { get => hButtonToDraw; set => hButtonToDraw = value; }

        public override string GetTypeString()
        {
            return "CustomTextField";
        }

        public override void Draw(Graphics g)
        {
            string visibleCaption = String.Empty;

            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;



            g.FillRectangle(backgroundBrush, rect);
            if (Caption.Length <= 29)
                visibleCaption = inputCaption;
            else
            {
                visibleCaption = inputCaption;
                visibleCaption = visibleCaption.Substring(visibleCaption.Length - 29);
            }
            g.DrawString(visibleCaption, inputFont, inputBrush, x + 3, y + 30);
            g.DrawString(upperCaption, upperFont, upperBrush, x + 8, y + 5);

            hButtonToDraw.Draw(g);
        }
    }
}
