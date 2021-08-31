using System;
using System.IO;
using System.Diagnostics;

namespace DomainAbstractions
{
    class LoggerToFile
    {
        public string InstanceName { get; set; } = "anonymous";
        private string filePath;
        private bool debugOutput;

        public LoggerToFile(string filePath = "", bool debug = false)
        {
            this.filePath = filePath;
            this.debugOutput = debug;
        }

        public void WriteLine(string content = "")
        {
            if (debugOutput)
            {
                Debug.WriteLine(content);
            }


            if (!string.IsNullOrEmpty(filePath))
            {
                // Safely writes string content to a file, and ensures that the stream is closed afterwards
                if (!File.Exists(filePath))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                    File.CreateText(filePath).Close();
                }
                try
                {
                    var writer = File.AppendText(filePath);
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
}


/*
Consider the problem of logging for domain abstractions.

Solutions fall into two design categories which exemplify the ALA design decision - is the logging _instance_ an abstraction.
In other words, should domain abstractiosn use _the_ one logging instance or should it have a port which needs to be wired individually for every instance of domain abstractions.
Note that in both cases we still have a dependency on logging as a concept - in other words there is a dependency on a logging interface - either implicit or explicit.
The reason this design decision is tricky is that in practice there may be hundreds or thousands of uses of the logging instance.
Surely that means it is abstract enough to have a dependency.
Note that by logging instance, it means a reference to the logging instance that resides in a static public global.

The ALA dependency rule is that you are allowed a dependency on a more abstract abstraction. But that statement is further clarified by saying it must be a knowledge dependency on a concept
that is need to understand the code.
Domain abstractions do not need knowledge of the concept of an instance of the logging to be understood, so from that rule we should use ports.
Other factors for using ports are reuse.
When a domain abstraction is reused say for a small example snippet, we don't really also want to have to provide a global instance reference, even if we can leave it null.
When a domain abstraction is reused in testing, we may want to test many in parallel. We don't really want them all to be using the same logging instance variable as the
tests could influence each other through that glabal, especially if we are also testing the logging functionality of the domain abstraction.

What if we want different domain abstractions or their instances to use different instances of logging. For example, we sometimes log the wiring output to a different log file from the run-time output.
So it's not even necessarily valid that there is only one logging instance.

On the other hand, the alternative is wiring up thousands of logging ports. In conventional code, this would be equivalent to "passing it" the logging instance into every object.
(Wiring is a way of doing dependency injection or passing in a concrete object.)

Before we look at how to solve the mass wiring problem, lets look at how the static global solution would be done.


Solution using a static global
------------------------------

I am guessing 90% of people would implement concrete logging as a static class.
This has the problem that there is only one implementation of logging.
It will likely have a lot of configuration to handle different types of logging.
The second problem is that static classes have inititlization by static initializers or static constructore. These are normally run by the system in an undefined order.
If you want one static class to use another during initialization, the used one may not have been initialized.

I am guessing 90% of the rest would use a singleton. This solves the intialization problem.
The singleton pattern is controversial because it is still a static global, it gives two reponsibilities to the class, one being that we are forcing only one instance.

I am guessing 90% of the rest create a separate logging interface, would instantiate the concrete logging class somewhere else higher in the application that knows what logging it wants
and would then store the logging instance in a static global reference for all classes to do their logging to.

I am guessing the rest would pass the logging instance to every single object that needed it, which, as ALA architects, is what we are going to do.
I guess there might be a small number who would use aspect oriented programming to do mass wiring, which is similar to what we are going to do.

In ALA applications, the last problem for the idea of an abstraction for the logging instance is that it would reside in the programming paradigms layer,
so programming paradigm abstractions themselves couldn't use it, and neither could the Foundation layer astractions.
This happens because the idea of a logging instance is not really a great abstraction.
However ports also have this problem becasue they need an interface. In our solution, the interface will be an implicit one that is very abstract (simpley any method that takes a single string paramater),
so this problem goes away.

The use of an explicit interface did have the advantage that it could provide sophisticated logging
methods that format the output better, automatically output the abstraction name, instance names and time stamps,
handle exception objects, etc.
However, this wide interface now presents its own problems becasue its not abstract enough for a programming paradigm - 
tend to change according to the needs of individual clients. 
So we wont be using an explcit interface for logging, just an impleit protype of a function that takes one string parameeter


So now lets look at how to do a logging port on every abstraction that needs logging.

Solution using a logging port
-----------------------------

This allows the domain abstractions to be reusable without dragging with it a static instance reference, although
the reference itself can be left unassigned (null).
We are using an implict interface, so an explicit interface is not needed. This is convenient not only as it allows reuse of abstraction without including the file with the interface definition,
but allows the foundation abstractions to also have logging ports.

We will typically create a logger abstraction for output to a text file, and one for outputting to the debug window.
We will typically want two instances of the LoggerToFile abstraction, one for wiring logging and one for runtime logging.

We will be able to either mass wire or individually wire abstractions to one or both of these loggers

This is a way to implement a logging port in a domain abstraction

        public delegate void DiagnosticOutputDelegate(string output);
        private static DiagnosticOutputDelegate diagnosticOutput;
        public static DiagnosticOutputDelegate DiagnosticOutput { get => diagnosticOutput; set => diagnosticOutput = value; }

Notice that these are static. This is makes the port belong to the class rather than the instances.
This allows wiring of every instance of a domain abstraction with one wiring operation.

Inside an abstraction, the port can be used like this
        diagnosticOutput?.Invoke($"AbstractionName.cs[this.InstanceName] {DateTime.Now:yyyy-MM-dd HH-mm-ss-ffff} message");

Note that we output the abstraction name and instance name and date first so we know where and when the logged messages originated.
(TBD is there a way to have the abstraction name, instance name and datetime automatically prepended by code in the logger abstraction?)

Wiring the ports can be as simple as:

Wiring.DiagnosticOutput += (s) => { Debug.WriteText(s); };
Wiring.DiagnosticOutput += (s) => { new LoggerToFile(@"C:\ProgramData\Example_ALA\wiringLog.txt") { InstanceName = "wiringLogging" }.WriteLine(s); };


Adding mass wiring of ports
---------------------------

Now the problem is that we have potenially hundreds of explicit wiring to do for logging ports.

The DiagnosticOutput ports on all the domain abstractions (not abstraction in lower layers) can all use the same name by convention.

Then we can make an abdtraction cabale of doing mass wiring almost like an aspect.

See WireMany for the solution to this.

*/


