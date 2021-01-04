using System;

namespace ProgrammingParadigms
{
    /// <summary>
    /// Sets up a global level debugging means to print logs.
    /// </summary>
    public static class Debug
    {
        static public bool GraphicsMode { get; } = false;
        static public bool ConsoleOutputMode = true;
        static public void WriteLine(string output)
        {
            if (ConsoleOutputMode)
            {
                var n = Indent;
                while (n-- > 0) Console.Write("    ");
                Console.WriteLine(output);
            }
        }

        private static int Indent = 0;

        static public void WriteLineIndent(string output)
        {
            WriteLine(output);
            Indent++;
        }

        static public void WriteLineUnindent(string output)
        {
            Indent--;
            WriteLine(output);
        }

        public static void Log<T>(this T A, string log)
        {
            string type = A.GetType().ToString();
            string instanceName = A.GetType().GetField("InstanceName").GetValue(A)?.ToString() ?? "Default";

            Debug.WriteLine($"{type} {instanceName} {log}");  
        }
    }
}
