using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engineering_Calculator
{
    public interface IExceptionObserver
    {
        void OnException(Exception e);
    }
}
