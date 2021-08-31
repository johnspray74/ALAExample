using System;

namespace ProgrammingParadigms
{
    // This class is an example of poor design
    // TBD not sure if we should keep this programming paradigm at all because Domain Abstractions don't, in their design, conceptually depend on it.
    // Therefore in ALA dependencies on these ara illegal, and wiring via port must be used instead.
    // That may seem burdensome within a single application but consider
    // when we reuse domain abstractions for other applications, for tiny example programs, for testing etc,
    // we don't necessarily want to have this class dragged along to allow it to complie.
    // At the very least we should create an interface for all th methods of this class, which can live here in Programming paradigms.
    // Then when you reuse/test domain abstraction that use it, you still have to have the interface, but nothing else.
    // Then change this to a non-static class that implements the interface.
    // Then every Domain abstraction that wants to use it should have a port, and check the port is wired before using.
    // Then, if the Application wants logging, it must instantiate this class and then wire all Domain Abstractions instances from which
    // it wants logging to it.
    // Finally this class must have a diagnosticsOutput style port similar to the one in Wiring.cs and the Application must wire that to a
    // suitable instance of LogToFile.cs and/or to System.Diagnostics.Debug.WriteLine as required.
    // Note that this abstraction would own the type of the diagnosticsOutput port, it wouldn't be an interface or delegate in a lower layer.

    // (Diagnosstic output can be done with a self owned delegate at the sending end and a ordinary method that takes a string at the
    // receiving end which are a little more light weight for the application to wire up.
    // If the sending end were to use an interface, the Application must have a wire in an adapter intermadiary object.
    // You cant do that with an anonymous class so the application must make a class - that heavy weight for what should be one line of code.)


    /// <summary>
    /// Sets up a global level debugging means to print logs.
    /// </summary>
    public static class DiagnosticWriteMethods
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
            string instanceName = A.GetType().GetField("InstanceName")?.GetValue(A)?.ToString() ?? "Default";

            System.Diagnostics.Debug.WriteLine($"{type} {instanceName} {log}");  
        }

        public static void Log<T>(this T A, string log, params object[] args)
        {
            Log(A, String.Format(log, args));
        }
    }
}
