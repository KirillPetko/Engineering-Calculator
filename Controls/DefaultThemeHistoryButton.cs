using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engineering_Calculator
{
    internal class DefaultThemeHistoryButton : CustomHistoryButton
    {
        public DefaultThemeHistoryButton() 
        {
            X = 697;
            Y = 33;
            Width = 25;
            Height = 25;
            Caption = "HistoryButton";

            pen = new Pen(Color.White, 3);
            border = new Rectangle(X, Y, Width, Height);

        }
        Rectangle border;
        Pen pen;

        public override void Draw(Graphics g)
        {
            g.DrawArc(pen, border, 170, 300);
            g.DrawLine(pen, X + 13, Y + 5, X + 13, Y + 12);
            g.DrawLine(pen, X + 13, Y + 12, X + 20, Y + 16);
        }
    }
}
