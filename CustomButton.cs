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
            X = 0;
            Y = 0;
            Width = 0;
            Height = 0;
            Caption = string.Empty;
        }
        public CustomButton(int _x, int _y, int _width, int _height, string _caption) 
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
        public override string GetTypeString()
        {
            return "CustomButton";
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
            border = new Rectangle(x, y, width, height);
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
