using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engineering_Calculator
{
    public interface IFormElementFactory
    {
        public CustomButton CreateButton(int _x, int _y, string _caption);
        public CustomTextField CreateTextField();
        public CustomHistoryButton CreateHistoryButton();
    }
}
