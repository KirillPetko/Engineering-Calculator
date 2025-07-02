using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Engineering_Calculator
{
    public class ExceptionHandler 
    {
        private readonly List<IExceptionObserver> _observers;
        public ExceptionHandler() 
        {
            _observers = new List<IExceptionObserver>();    
        }

        public void AddObserver(IExceptionObserver observer)
        { 
            _observers.Add(observer);
        }

        public void RemoveObserver(IExceptionObserver observer)
        {
            _observers.Remove(observer);
        }

        public void HandleException(Exception e)
        {
            foreach (IExceptionObserver observer in _observers)
                observer.OnException(e);
        }
    }
}
