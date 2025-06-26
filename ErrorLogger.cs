using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engineering_Calculator
{
    internal class ErrorLogger : IExceptionObserver
    {
        public ErrorLogger() 
        { 
            fileManager = new FileManager(); 
            msg = String.Empty;
        }
        FileManager fileManager;
        string msg;
        public void OnException(Exception e)
        {
            bool isDeterminedException = e.Data["DateTimeInfo"] != null && e.Data["UserInput"] 
                                         != null && e.Data["CausingExpression"] != null;
            string dateMsgSeparator = "\t\t";
            if (isDeterminedException)
            {
                string dataSeparator = String.Empty;
                int userInputLength = e.Data["UserInput"].ToString().Length;
                if (e.Message == "Wrong operands format")
                    dateMsgSeparator = "\t";
                if (userInputLength <= 7)
                    dataSeparator = "\t\t\t\t\t\t";
                if (userInputLength >= 8 && userInputLength <= 14)
                    dataSeparator = "\t\t\t\t\t";
                if (userInputLength >= 15 && userInputLength < 24)
                    dataSeparator = "\t\t\t\t";
                if (userInputLength >= 24 && userInputLength < 32)
                    dataSeparator = "\t\t\t";
                if (userInputLength >= 32 && userInputLength < 42)
                    dataSeparator = "\t\t";
                if (userInputLength >= 42)
                    dataSeparator = "\t";

                msg = e.Data["DateTimeInfo"] + "\t" + e.Message + dateMsgSeparator + e.Data["UserInput"];
                msg += dataSeparator + ">>>>>" + "\t\t" + e.Data["CausingExpression"];
            }
            else
            {
                e.Data.Add("DateTimeInfo", DateTime.Now);
                msg = e.Data["DateTimeInfo"] + "\t" + e.Message + "\t" + e.GetType();
            }
            fileManager.LogException(msg);
        }
    }
}
