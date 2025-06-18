using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engineering_Calculator
{
    internal class ErrorLogger : IExceptionObserver
    {
        public void OnException(Exception e)
        {
            //throw new NotImplementedException();
        }
    }
}
