using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Engineering_Calculator
{
    internal static class FileManager
    {
        public static void LogException(string msg)
        {
            using (StreamWriter sw = File.AppendText("log.txt"))
            {
                sw.WriteLine(msg);
            }
        }

        public static void RecordCalculation(string expression)
        {
            using (StreamWriter sw = File.AppendText("history.txt"))
            {
                sw.WriteLine(expression);
            }
        }
        public static void OpenFile(string command)
        {
            string errorMsg = String.Empty, fileName = String.Empty;
            switch (command)
            {
                case "-history":
                    errorMsg = "Absent history";
                    fileName = "history.txt";
                    break;
                case "-log":
                    errorMsg = "Absent log";
                    fileName = "log.txt";
                    break;
                default:
                    break;
            }
            string path = fileName;
            if (System.IO.File.Exists(path))
                Process.Start(path);
            else
            {
                Exception e = ErrorFactory.CreateFileException(errorMsg, command, fileName);
                throw e;
            }
        }
    }
}
