using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engineering_Calculator
{
    internal class DefaultThemeFactory : IFormElementFactory
    {
        public CustomButton CreateButton(int _x, int _y, string _caption)
            => new DefaultThemeButton(_x, _y, _caption);
        public CustomTextField CreateTextField()
            => new DefaultThemeTextField();
        public CustomHistoryButton CreateHistoryButton()
            => new DefaultThemeHistoryButton();
    }
}
