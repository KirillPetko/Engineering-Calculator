using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace Engineering_Calculator
{
    internal class DefaultThemeButton : CustomButton
    {
        public DefaultThemeButton(int _x, int _y, string _caption) 
        {
            X = _x;
            Y = _y;
            Width = 75;
            Height = 75;
            Caption = _caption;

            if (Caption == "C" || Caption == "<-")
                brush = new SolidBrush(Color.Red);
            else
                brush = new SolidBrush(Color.Black);
            font = new System.Drawing.Font("Consolas", (float)17);
            pen = new Pen(Color.Black, 1);
            border = new Rectangle(X, Y, Width, Height);

        }
        private readonly Rectangle border;
        private readonly Brush brush;
        private readonly Font font;
        private readonly Pen pen;
        public override void Draw(Graphics g)
        {
            g.DrawRectangle(pen, border);
            DrawCaptions(g);
        }

        public void DrawCaptions(Graphics g)
        {
            //centring Caption in button rectangle by its length
            if (Caption.Length == 1)
                g.DrawString(Caption, font, brush, X + 27, Y + 25);
            else if (Caption.Length == 2)
                g.DrawString(Caption, font, brush, X + 22, Y + 25);
            else if (Caption.Length == 3)
                g.DrawString(Caption, font, brush, X + 15, Y + 25);
            else if (Caption.Length == 4)
                g.DrawString(Caption, font, brush, X + 8, Y + 25);

        }
    }
}
