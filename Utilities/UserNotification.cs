using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Engineering_Calculator
{
    internal class UserNotification : IExceptionObserver
    {
        public UserNotification() { }
        
        public UserNotification(CustomTextField _textField)
        {
            TextField = _textField;
        }
        private CustomTextField textField;
        public CustomTextField TextField { get => textField; set => textField = value; }

        public void OnException(Exception e)
        { 
            TextField.Caption = e.Message;
        }
    }
}
