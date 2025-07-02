using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace Engineering_Calculator
{
    public abstract class CustomHistoryButton : FormElement
    {
        public CustomHistoryButton()
        {
            X = 697;
            Y = 33;
            Width = 25;
            Height = 25;
            Caption = "HistoryButton";
        }

        public override int X { get; set; }
        public override int Y { get; set; }
        public override int Width { get; set; }
        public override int Height { get; set; }
        public override string Caption { get; set; }
        public override string GetTypeString()
        {
            return Caption;
        }
        public abstract override void Draw(Graphics g);
    }
}
