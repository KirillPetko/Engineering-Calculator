using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engineering_Calculator
{
   //class responsible for visual representation of a custom button
    internal class CustomButton:FormElement
    {
        public CustomButton()
        {
            x = 0;
            y = 0;
            width = 0;
            height = 0;
            caption = string.Empty;
        }
        public CustomButton(int _x, int _y, int _width, int _height, string _caption) 
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
                if(value == String.Empty)
                    throw new NotImplementedException("parameter has to differ from String.Empty to be set![Caption]");
                else caption = value;

            }
        }

        public override void Draw(Graphics g)
        {
            Rectangle border;
            Brush brush;
            Font fnt;
            Pen pen;

            if (caption == "C" || caption == "<-")
                brush = new SolidBrush(Color.Red);
            else
                brush = new SolidBrush(Color.Black);
            fnt = new System.Drawing.Font("SansSerif", (float)15);
            pen = new Pen(Color.Black, 1);
            border = new Rectangle(X, Y, Width, Height);
            g.DrawRectangle(pen, border);

            //centring caption in button rectangle by caption length
            if (caption.Length == 1) 
                g.DrawString(caption, fnt, brush, x + 30, y+25);
            else if (caption.Length == 2)
                g.DrawString(caption, fnt, brush, x + 25, y + 25);
            else if(caption.Length == 3)
                g.DrawString(caption, fnt, brush, x + 20, y + 25);
            else if(caption.Length == 4)
                g.DrawString(caption, fnt, brush, x + 15, y + 25);

            brush.Dispose();
            fnt.Dispose();
            pen.Dispose();
        }
    }
}
