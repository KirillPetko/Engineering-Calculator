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
    public abstract class CustomTextField:FormElement
    {
        public CustomTextField() 
        {
            X = 50;
            Y = 30;
            Width = 675; 
            Height = 80;
            Caption = String.Empty;
            UpperCaption = String.Empty;    
        }

        public override int X { get; set; }
        public override int Y { get; set; }
        public override int Width { get; set; }
        public override int Height { get; set; }
        public override string Caption { get; set; }
        public string UpperCaption { get; set; }
        public FormElement HButtonToDraw { get; set; }

        public override string GetTypeString()
        {
            return "CustomTextField";
        }
        public abstract override void Draw(Graphics g);
    }
}
