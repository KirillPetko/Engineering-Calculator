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
            Y = 50;
            Width = 675; 
            Height = 50;
            Caption = String.Empty;
        }
        public CustomTextField(int _x, int _y, int _width, int _height, string _caption)
        {
            X = _x;
            Y = _y;
            Width = _width;
            Height = _height;
            Caption = _caption;
        }

        private int x;
        private int y;
        private int width;
        private int height;
        private string caption;

        public override int X { get => x; set => x = value; }
        public override int Y { get => y; set => y = value; }
        public override int Width { get => width; set => width = value; }
        public override int Height { get => height; set => height = value; }
        public override string Caption { get => caption; set => caption = value; }

        public override void Draw(Graphics g)
        {
            Rectangle rect;
            Brush brush;
            Brush brush1;
            Font fnt;

            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;

            rect = new Rectangle(x, y, width, height);
            brush = new SolidBrush(Color.Black);
            brush1 = new SolidBrush(Color.Orange);
            fnt = new System.Drawing.Font("Consolas", (float)20);//even character length

            g.FillRectangle(brush, rect);
            if (Caption.Length <= 43)
                g.DrawString(Caption, fnt, brush1, x + 10, y + 5);
            else
            {
                string visibleCaption = caption;
                visibleCaption = visibleCaption.Substring(visibleCaption.Length - 43);
                g.DrawString(visibleCaption, fnt, brush1, x + 10, y + 5);
            }
            fnt.Dispose();
            brush.Dispose();
            brush1.Dispose();
        }
    }
}
