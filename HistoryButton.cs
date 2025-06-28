using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace Engineering_Calculator
{
    internal class HistoryButton : FormElement
    {
        public HistoryButton()
        {
            X = 697;
            Y = 33;
            Width = 25;
            Height = 25;
            Caption = "HistoryButton";

            brush = new SolidBrush(Color.White);
            pen = new Pen(Color.White, 3);
            border = new Rectangle(x, y, width, height);
        }

        private int x;
        private int y;
        private int width;
        private int height;
        private string caption;

        Rectangle border;
        Brush brush;
        Pen pen;

        public override int X { get => x; set => x = value; }
        public override int Y { get => y; set => y = value; }
        public override int Width { get => width; set => width = value; }
        public override int Height { get => height; set => height = value; }
        public override string Caption { get => caption; set => caption = value; }
        public override string GetTypeString()
        {
            return Caption;
        }
        public override void Draw(Graphics g)
        {
            g.DrawArc(pen, border, 170, 300);
            g.DrawLine(pen, x+13, y+5, x+13, y+12);
            g.DrawLine(pen, x + 13, y + 12, x + 20, y + 16);
        }
    }
}
