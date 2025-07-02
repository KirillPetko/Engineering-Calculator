using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engineering_Calculator
{
    internal class DarkThemeButton : CustomButton
    {
        public DarkThemeButton(int _x, int _y, string _caption)
        {
            X = _x;
            Y = _y;
            Width = 75;
            Height = 75;
            Caption = _caption;

            if (Caption == "C" || Caption == "<-")
                brush = new SolidBrush(Color.FromArgb(199, 54, 89));
            else
                brush = new SolidBrush(Color.FromArgb(238, 238, 238));
            font = new System.Drawing.Font("Consolas", (float)17);
            pen = new Pen(Color.MintCream, 1);
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
