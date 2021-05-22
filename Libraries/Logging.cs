using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libraries
{
    public static class Logging
    {
        public static string logFilePath = @"C:\ProgramData\Example_ALA\DataLink_ALA.log";
        public static string wiringLogFilePath = @"C:\ProgramData\Example_ALA\wiringLog.txt";
        public static string lineSeparator = new string('-', 120) + "\n";

        public static string GetCurrentTime()
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
        
        [Conditional("DEBUG")]
        public static void WriteToWiringLog()
        {
            // Overload for printing a separator between wiring outputs
            WriteText(path: wiringLogFilePath, content: "");
        }

        public static string WriteToWiringLog(dynamic A, dynamic B, dynamic matchedInterface, bool save = true)
        {
            string AInstanceName = "(No InstanceName)";
            string BInstanceName = "(No InstanceName)";
            try { if (A.InstanceName != null) AInstanceName = $"(\"{A.InstanceName}\")"; } catch { };
            try { if (B.InstanceName != null) BInstanceName = $"(\"{B.InstanceName}\")"; } catch { };

            var AClassName = A.GetType().Name;
            var BClassName = B.GetType().Name;
            string matchedInterfaceType = $"{matchedInterface.FieldType.Name}";
            if (matchedInterface.FieldType.GenericTypeArguments.Length > 0)
            {
                matchedInterfaceType += $"<{matchedInterface.FieldType.GenericTypeArguments[0]}>"; 
            }

            string output = $"({AClassName} {AInstanceName}.{matchedInterface.Name}) -- [{matchedInterfaceType}] --> ({BClassName} {BInstanceName})";
            if (save) WriteText(path: wiringLogFilePath, content: output);
            return output; 
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
