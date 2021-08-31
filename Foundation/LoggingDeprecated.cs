using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foundation
{
    public static class LoggingDeprecated
    {
        public static string lineSeparator = new string('-', 120) + "\n";
        public static string logFilePath { get;  set; }

        private static string GetCurrentTime()
        {
            return $"{DateTime.Now:yyyy-MM-dd HH-mm-ss-ffff}";
        }

        public static void Log(Exception e)
        {
            string message = $"{lineSeparator}[{GetCurrentTime()}] Exception thrown: \n-> {e.Source} | {e.TargetSite}:\n-> \"{e.Message}\"\n-> StackTrace:\n{e.StackTrace}\n{lineSeparator}";
            WriteText(path: logFilePath, content: message);
        }

        public static void Log(object exceptionObject)
        {
            string message = $"{lineSeparator}[{GetCurrentTime()}] Unknown exception thrown: \n-> {exceptionObject}\n{lineSeparator}";
            WriteText(path: logFilePath, content: message);
        }

        [Conditional("DEBUG")]
        public static void Log(string s)
        {
            WriteText(path: logFilePath, content: $"{lineSeparator}[{GetCurrentTime()}]\n{s}\n{lineSeparator}");
        }
        


        public static void WriteText(string path = "", string content = "", bool createNewFile = false)
        {
            // Safely writes string content to a file, and ensures that the stream is closed afterwards
            if (createNewFile) File.CreateText(path).Close();
            try
            {
                var writer = File.AppendText(path);
                writer.WriteLine(content);
                writer.Close();
            }
            catch (Exception e) // File is already open
            {
                Debug.WriteLine(e.Message);
            }
        }
    }
}
