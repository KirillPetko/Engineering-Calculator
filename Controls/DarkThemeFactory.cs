using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engineering_Calculator
{
    internal class DarkThemeFactory : IFormElementFactory
    {
        public CustomButton CreateButton(int _x, int _y, string _caption)
    => new DarkThemeButton(_x, _y, _caption);
        public CustomTextField CreateTextField()
            => new DarkThemeTextField();
        public CustomHistoryButton CreateHistoryButton()
            => new DarkThemeHistoryButton();
    }
}
