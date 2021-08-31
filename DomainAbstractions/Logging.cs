using System;
using System.IO;
using System.Diagnostics;

namespace DomainAbstractions
{
    class Logging
    {
        public string InstanceName { get; set; } = "anonymous";
        private string filePath;
        private bool debugOutput;

        public Logging(string filePath = "", bool debug = true)
        {
            this.filePath = filePath;
            this.debugOutput = debug;
        }

        public void WriteText(string content = "")
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

Solution 1: LoggingServiceInstance

It is so easy to slam in a static logging abstraction in the programming paradigms layer
and justify that by saying logging is more abstract than domain abstractions - it probably is.

Lets see how this logging service would be implemented if we wnt that way.
A Logging service in the programming paradigms would have a single public instance of a logging class (not a singleton),
as a property of a ststic class. It would have the type of a public logging interface.
The Application would instantiate a concrete instance according to its logging requirements and plug it into the global instance variable's the setter.
All the domain abstraction instances use the getter on this LoggingServiceInstance static class.

Apart from relieving the application of wiring up logging to every instance of domain abstractions (thousands),
this solution has the advantage that the interface can provide sophisticated logging
methods that format the output better, automatically output the abstraction name, instance names and time stamps,
handle exception objects, etc.
However, this wide interface now presents its own problems becasue its not abstract enough for a programming paradigm.
It would change according to the needs of the clients. 

The fundamental ALA rule is that if the actual design and understanding of the code depends on a concept,
then you can have a dependency on an abstraction in a lower layer for that concept, otherwise use a port and wire it.
If you are still unsure, do the test test. When you are unit testing the domain abstraction, do you want it to drag the logging dependency with it?
You must test the logging via the logging service - you can plug a test concrete logger. But you cant test multiple domain abstractions instances
in parallel. This is not so good having this dependency on a global.

Another problem is that you have to have the LoggingService class in every tiny example code snippet that uses the domain abstraction
when the domain abstraction itself does not inherently need it, especially in an example. 
We always need small example programs, even if they are to demonstrate what a domain abstraction does.
You dont have to plug in a concrete logger, you can leave it empty, but the LoggingServiceInstance class and logging interface must be present.

The last problem for the LoggingServiceInstance is that abstractions in the ProgrammingParadigms and Foundations layers cant use it.


Solution 2: Logging port on every abstraction

So now lets describe the solution using a logging port on every domain abstraction.

This allows the domain abstractions to be reusable without dragging in LoggingServiceInstance.cs. 
The application wire each and every instance of domain abstraction needing logging to an instance of this Logging.cs class.
Of it can wire them somewhere else.

It can wire them to different logging channels, something we couldn't easily do with the 
LoggingServiceInstance solution. We already normally have two instances of this class, one for diagnostic logging of wiring and one for diagnostic
logging at run-time.
They are wired to different log files, although when going to the debug out window, they both go to the same window.
So the fact that we have two instances is a smell that we shouldn't have a logging service abstraction.

This is a common way to implement a logging port in a domain abstraction

        public delegate void DiagnosticOutputDelegate(string output);
        private static DiagnosticOutputDelegate diagnosticOutput;
        public static DiagnosticOutputDelegate DiagnosticOutput { get => diagnosticOutput; set => diagnosticOutput = value; }

Notice that these are static. This is how we make the port belong to the class rather than the instances.
We only need to wire each domain abstraction class that needs logging, not every instance.
That cuts the amount of logging wiring from thousands to hundreds.

And use it like this:
        diagnosticOutput?.Invoke($"AbstractionName.cs[this.InstanceName] message");

This is a better solution that solution 1 and what is currently implemented.
You can see the wiring being done in Application.cs at the end of the constructor

Solution 3: Default automated wiring

If we want to do applications without wiring up all the logging ports of the domain abstractions one by one,
and without requiring the presence of the LoggingServiceIinstance:
We would need a way for DomainAbstractions to use logging if its available, but not complain if it's completely missing (unlike the interface instance idea above)

The DiagnosticOutput ports on all the domain abstractions (not abstraction in lower layers) are given an attribute to mark it as
a logging port.

The Logging.cs abstraction, (this file) which is the concrete logger in the Domain Abstraction layer, has a configuration method.
The application can tell it to go and find all the logging ports and wire them to itself.
The application can then change the wiring from this default if needed.

(Conditional Atrributes when the class is completely missing wont work.)
Maybe there is still a way to do this.
*/


