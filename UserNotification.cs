using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engineering_Calculator
{
    internal class UserNotification : IExceptionObserver
    {
        public UserNotification() { }
        
        public UserNotification(FormElement element)
        {
            textField = element;
        }
        FormElement textField;
        public void OnException(Exception e)
        { 
            textField.Caption = e.Message;
        }
    }
}
