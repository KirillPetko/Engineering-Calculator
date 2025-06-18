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
            x = 50;
            y = 50;
            width = 675; 
            height = 50;
            caption = String.Empty;
        }
        public CustomTextField(int _x, int _y, int _width, int _height, string _caption)
        {
            x = _x;
            y = _y;
            width = _width;
            height = _height;
            caption = _caption;
        }

        private int x;
        private int y;
        private int width;
        private int height;
        private string caption;

        public override int X 
        {
            get 
            {
                return x;
            }
            set
            {
                if (value < 0)
                    throw new NotImplementedException("parameter has to be => 0 to be set![X]");
                else x = value;
            }
        }
        public override int Y
        {
            get
            {
                return y;
            }
            set
            {
                if (value < 0)
                    throw new NotImplementedException("parameter has to be => 0 to be set![Y]");
                else y = value;
            }
        }
        public override int Width
        {
            get
            {
                return width;
            }
            set
            {
                if (value < 0)
                    throw new NotImplementedException("parameter has to be => 0 to be set![Width]");
                else width = value;
            }
        }

        public override int Height
        {
            get
            {
                return height;
            }
            set
            {
                if (value < 0)
                    throw new NotImplementedException("parameter has to be => 0 to be set![Height]");
                else height = value;
            }
        }
        public override string Caption
        {
            get
            {
                return caption;
            }
            set
            {
                caption = value;
            }
        }

        public override void Draw(Graphics g)
        {
            Rectangle rect;
            Brush brush;
            Brush brush1;
            Font fnt;

            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;

            rect = new Rectangle(X, Y, Width, Height);
            brush = new SolidBrush(Color.Black);
            brush1 = new SolidBrush(Color.Orange);
            fnt = new System.Drawing.Font("Consolas", (float)20);//even character length

            g.FillRectangle(brush, rect);
            if (Caption.Length <= 43)
                g.DrawString(Caption, fnt, brush1, x + 10, y + 5);
            else
            {
                string visibleCaption = Caption;
                visibleCaption = visibleCaption.Substring(visibleCaption.Length - 43);
                g.DrawString(visibleCaption, fnt, brush1, x + 10, y + 5);
            }
            fnt.Dispose();
            brush.Dispose();
            brush1.Dispose();
        }
    }
}
