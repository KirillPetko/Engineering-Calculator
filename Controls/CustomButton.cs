using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engineering_Calculator
{
   //class responsible for an abstract representation of a button
    public abstract class CustomButton:FormElement
    {
        public CustomButton()
        {
        }
        public CustomButton(int _x, int _y, string _caption) 
        {
            X = _x;
            Y = _y;
            Width = 75;
            Height = 75;
            Caption = _caption;
        }

        public override int X { get; set; }
        public override int Y { get; set; }
        public override int Width { get; set; }
        public override int Height { get; set; }
        public override string Caption { get; set; }

        public override string GetTypeString()
        {
            return "CustomButton";
        }
        public abstract override void Draw(Graphics g);
    }
}
