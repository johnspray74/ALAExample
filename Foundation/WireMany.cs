using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Foundation
{
    static class WireMany
    {
        /// <summary>
        /// WireMany can wire up many ports at once to a destination. 
        /// The source port must match a convention such as its name.
        /// This solves the problem of the application having to individually wire hundreds or thousands of ports to the same place.
        /// An example application is logging output ports (See logging.cs for more design explanation of why we use ports for logging instead of a logging static class abstraction or logging global static instance abstraction.)
        /// </summary>



        /// <summary>
        /// This method wires a named static port in many abstractions to a destination object
        /// (A static port is used by all instances of the abstraction.)
        /// The destination object must implement the same interface of the field or property of the port
        /// The source abstraction classes must be in the specified namespace e.g. "DomainAbstractions"
        /// If its a field it must be private (as is the convention for ALA ports)
        /// </summary>
        /// <param name="nameSpace"></param>
        /// <param name="fieldOrPropertyName"></param>
        /// <param name="destinationObject"></param>
        public static void WireManyPortsTo(string nameSpace, string fieldOrPropertyName, object destinationObject)
        {
            // find all classes in the specified namespace
            var classes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => String.Equals(t.Namespace, nameSpace, StringComparison.Ordinal));

            foreach (var cl in classes)
            {
                // diagnosticOutput?.Invoke($"Type {cl.Name}");
                // see if the class has the specified field, the field is an interface type, the field isn't already wired,
                // and if the interface type matches an interface implemented by the destinationObject
                // wire it through
                var fieldInfo = cl.GetField(fieldOrPropertyName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
                if (fieldInfo != null)
                {
                    Type sourceType = fieldInfo.FieldType;
                    var destinationType = destinationObject.GetType();
                    var destinationInterfaceType = destinationObject.GetType().GetInterfaces().Where((i) => sourceType == i).ToArray();
                    if (destinationInterfaceType.Length == 1)
                    {
                        fieldInfo.SetValue(null, destinationObject);
                        diagnosticOutput?.Invoke($"Wired {cl.Name}.{fieldInfo.Name} to {destinationObject.GetType().Name}");
                    }
                    // TBD should we also handle a list like we do in the WireTo method?
                }
            }
        }





        /// <summary>
        /// This method wires the named static event port of many abstractions to a destination object method
        /// (A static port is used by all instances of the abstraction.)
        /// The source port must be in the form of an event (either public or private).
        /// The destination object must have a public method with the same signature of the delegate of the event
        /// The source abstraction classes must be in the specified namespace e.g. "DomainAbstractions"
        /// If the port is static, the same port is used by every instance of the class. 
        /// This type of wiring does not use an explicit interface - it wires directly an event to a method in the destination if the methods are compatible
        /// In other words there is an implicit interface for the method signature, so there is no type check of that signature at compiler time.
        /// This allows reuse of abstractions without having to include an explicit interface definition, which would be the delegate.
        /// 
        /// (This type of wiring is also suitable where there is no implicit interface e.g. the source signature and detination signature do not match
        /// In such cases an adapter pattern could be used. The application creates an adpater that matches the signature of the source and contains a port that matches the signature of the destination.
        /// Then it can use this method to wire many abstractions or instances to an instance of the adapter and manually wire the adapter to the destination.
        /// ALA discourages adpater becasue it discourages classes owning their interfaces. One of ALA fundamental premises is abstract interfaces, not owned interfaces.
        /// The problem with owned interfaces is the tempatation to wire them without an adapter - thus making the two abstractions coupled. One can start doing what the other specifically requires.)
        /// </summary>
        /// <param name="nameSpace"></param>
        /// <param name="eventName"></param>
        /// <param name="destinationObject"></param>
        /// <param name="destinationMethodName"></param>
        public static void WireManyTo(string nameSpace, string eventName, object destinationObject, string destinationMethodName)
        {
            var classes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => String.Equals(t.Namespace, nameSpace, StringComparison.Ordinal));

            foreach (var cl in classes)
            {
                // diagnosticOutput?.Invoke($"Type {cl.Name}");
                var eventInfo = cl.GetEvent(eventName, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);  
                if (eventInfo != null)
                {
                    Type tDelegate = eventInfo.EventHandlerType;
                    Delegate d = Delegate.CreateDelegate(tDelegate, destinationObject, destinationMethodName);
                    eventInfo.AddEventHandler(null, d);
                    diagnosticOutput?.Invoke($"Wired event {cl.Name}.{eventInfo.Name} to {destinationObject.GetType().Name}.{destinationMethodName}");
                }
            }

        }


        /// TBD firgure out how to find all the objects of a class so we can wire individual instances this way
        /// This is not supported by reflection because it apparently violates privacy. (Well all reflection can do that - not sure I understand the inconsistency.)
        /// So there is no Type.getInstances()
        /// What we are trying to do is analogous to aspects - an example cross-cutting concern is logging, but we don't want to create a dependency on a
        /// static logging class or a static logging instance global
        /// we want to use a port, but there are many of them to wire so we want to mass wire them.
        /// 
        /// To work around this, every abstraction that wants to allow its instances to be individually wired or mass wired will need to have, in their constructor,
        /// code that adds their own 'this' reference to a static list.
        /// Then these methods can use reflection to see if that static list is present.
        /// In the meantime, only static ports are supported for mass wiring.


        // output port for logging every wiring the WireMany does
        public delegate void DiagnosticOutputDelegate(string output);
        public static event DiagnosticOutputDelegate diagnosticOutput;

    }
}
