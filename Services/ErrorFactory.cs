using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engineering_Calculator
{
    //responsible for forming custom exceptions containing specific data
    internal static class ErrorFactory
    {
        static ErrorFactory() { }
        static Exception e;

        //supposed to be called during calculation process
        public static Exception CreateCalculationException(string _message, string _exceptionType, string _causingExpression, string _userInput)
        {
            switch (_exceptionType)
            {
                case "StackOverflowException":
                    e = new StackOverflowException(_message);
                    break;
                case "ArithmeticException":
                    e = new ArithmeticException(_message);
                    break;
                case "FormatException":
                    e = new FormatException(_message);
                    break;
                default:
                    e = new NotImplementedException(_message);
                    break;
            }
            e.Data.Add("DateTimeInfo", DateTime.Now);
            e.Data.Add("CausingExpression", _causingExpression);
            e.Data.Add("UserInput", _userInput);
            return e;
        }
        public static Exception CreateFileException(string _message, string command, string fileName)
        {
            e = new FileNotFoundException(_message);
            e.Data.Add("DateTimeInfo", DateTime.Now);
            e.Data.Add("CausingExpression", fileName);
            e.Data.Add("UserInput", command);
            return e;
        }
    }
}
