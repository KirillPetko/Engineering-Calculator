using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engineering_Calculator
{
    internal class DefaultThemeTextField : CustomTextField
    {
        public DefaultThemeTextField() 
        {
            X = 50;
            Y = 30;
            Width = 675;
            Height = 80;
            Caption = String.Empty;
            UpperCaption = String.Empty;

            rect = new Rectangle(X, Y, Width, Height);
            backgroundBrush = new SolidBrush(Color.Black);
            inputBrush = new SolidBrush(Color.Orange);
            upperBrush = new SolidBrush(Color.LimeGreen);
            inputFont = new Font("Consolas", 30);
            upperFont = new Font("Consolas", 15);

        }
        private readonly Rectangle rect;
        private readonly Brush backgroundBrush, inputBrush, upperBrush;
        private readonly Font inputFont, upperFont;

        public override void Draw(Graphics g)
        {
            string visibleCaption = String.Empty;

            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;



            g.FillRectangle(backgroundBrush, rect);
            if (Caption.Length <= 29)
                visibleCaption = Caption;
            else
            {
                visibleCaption = Caption;
                visibleCaption = visibleCaption.Substring(visibleCaption.Length - 29);
            }
            g.DrawString(visibleCaption, inputFont, inputBrush, X + 3, Y + 30);
            g.DrawString(UpperCaption, upperFont, upperBrush, X + 8, Y + 5);

            HButtonToDraw.Draw(g);
        }
    }
}
