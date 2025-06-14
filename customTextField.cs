﻿using System;
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
    //class responsible for graphical representation of a custom text field on form
    internal class customTextField:FormElement
    {
        public customTextField() 
        {
            x = 50;
            y = 50;
            width = 675; 
            height = 50;
            caption = String.Empty;
        }
        public customTextField(int _x, int _y, int _width, int _height, string _caption)
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
            Rectangle rect = new Rectangle(X, Y, Width, Height);
            Brush brush = new SolidBrush(Color.Black);
            g.FillRectangle(brush, rect);
            Brush brush1 = new SolidBrush(Color.Orange);

            //Font with even character length
            Font fnt = new System.Drawing.Font("Consolas", (float)20);
            if (Caption.Length <= 43)
                g.DrawString(Caption, fnt, brush1, x + 10, y + 5);
            else 
            {
                string visibleCaption = Caption;
                visibleCaption = visibleCaption.Substring(visibleCaption.Length-43);
                g.DrawString(visibleCaption, fnt, brush1, x + 10, y + 5);
            }
        }
    }
}
