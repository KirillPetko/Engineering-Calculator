using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Engineering_Calculator
{
    internal class FileManager
    {
        public void LogException(string msg)
        {
            using (StreamWriter sw = File.AppendText("log.txt"))
            {
                sw.WriteLine(msg);
            }
        }

        public void RecordCalculation(string expression)
        {
            using (StreamWriter sw = File.AppendText("history.txt"))
            {
                sw.WriteLine(expression);
            }

        }
    }
}
