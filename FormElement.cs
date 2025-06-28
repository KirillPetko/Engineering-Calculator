using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Engineering_Calculator
{
    //abstract class to define custom form elemets behavior
    abstract class FormElement
    {
        public FormElement()
        {
        } 

        //since each element on form will be represented by rectangle,
        //there are cords of upper right corner, widht and height fields 
        public abstract int X { get; set; }
        public abstract int Y { get; set; }
        public abstract int Width { get; set; }
        public abstract int Height { get; set; }
        public abstract string Caption { get; set; }
        public abstract string GetTypeString();
        public abstract void Draw(Graphics g);
        public bool CheckPoint(int pointX, int pointY)
        {
            return (pointX > X && pointY > Y) && (pointX < X + Width && pointY < Y + Height) ? true : false;
        }
    }
}
