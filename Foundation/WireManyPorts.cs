using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Foundation
{
    class WireManyPorts
    {
        /// <summary>
        /// WireManyPorts can wire up many ports at once to a destination. 
        /// The source port must match a convention such as its name.
        /// This solves the problem of the application having to individually wire hundreds or thousands of ports to the same place.
        /// An example application is logging output ports (See logging.cs for more design explanation of why we use ports for loggin.)
        /// </summary>
        public string InstanceName { get; set; } = "anonymous";  // name for this instance of WireManyPorts



        /// <summary>
        /// This method wires a named static port of many abstractions to a destination object
        /// or wires a named port of many instances of many abstractions to a destination object
        /// (A static port is used by all instances of the abstraction.)
        /// The destination object must implement the same interface of the field or property of the port
        /// The source abstraction classes must be in the specified namespace e.g. "DomainAbstractions"
        /// If its a field it must be private (as is the convention for ALA ports)
        /// </summary>
        /// <param name="nameSpace"></param>
        /// <param name="fieldOrPropertyName"></param>
        /// <param name="destinationObject"></param>
        public void WireManyPortsTo(string nameSpace, string fieldOrPropertyName, object destinationObject)
        {
            // find all classes in the specified namespace
            var classes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => String.Equals(t.Namespace, nameSpace, StringComparison.Ordinal));

            foreach (var cl in classes)
            {
                // diagnosticOutput($"Type {cl.Name}");
                // see if the class has the specified property, the property is an interface type, the property isn't already wired,
                // and if the interface type matches an interface implemented by the destinationObject
                // wire it through
                var propertyInfo = cl.GetProperty(fieldOrPropertyName);
                if (propertyInfo != null)
                {
                    Type sourceType = propertyInfo.PropertyType;
                    if (sourceType.IsInterface && propertyInfo.GetValue(null) == null)  // don't wire the port if its already wired
                    {
                        var destinationType = destinationObject.GetType();
                        var destinationInterfaceType = destinationObject.GetType().GetInterfaces().Where((i) => sourceType == i).ToArray();
                        if (destinationInterfaceType.Length==1)
                        {
                            propertyInfo.SetValue(null, destinationObject);
                            diagnosticOutput($"Wired {cl.Name}.{propertyInfo.Name} to {destinationObject.GetType().Name}");
                        }
                    }
                }
                else
                {
                    // see if the class has the specified field, the field is an interface type, the field isn't already wired,
                    // and if the interface type matches an interface implemented by the destinationObject
                    // wire it through
                    var fieldInfo = cl.GetField(fieldOrPropertyName);
                    if (fieldInfo != null)
                    {
                        Type sourceType = fieldInfo.FieldType;
                        var destinationType = destinationObject.GetType();
                        var destinationInterfaceType = destinationObject.GetType().GetInterfaces().Where((i) => sourceType == i).ToArray();
                        if (destinationInterfaceType.Length == 1)
                        {
                            fieldInfo.SetValue(null, destinationObject);
                            diagnosticOutput($"Wired {cl.Name}.{fieldInfo.Name} to {destinationObject.GetType().Name}");
                        }
                        // TBD should we also handle a list like we do in the WireTo method?
                    }
                }
            }
        }





        /// <summary>
        /// This method wires a named static port of many abstractions to a destination object method
        /// or wires a named port of many instances of many abstractions to a destination object method
        /// (A static port is used by all instances of the abstraction.)
        /// The source field or property must be a delegate or event.
        /// The destination object must have a public method with the same signature of the delegate or event
        /// The source abstraction classes must be in the specified namespace e.g. "DomainAbstractions"
        /// If its a field it must be private (as is the convention for ALA ports)
        /// If the port is static, the same port is used by every instance of the class. 
        /// This type of wiring does not use an explicit interface - it wires directly a single method to a method in the destination
        /// There is an implicit interface that the signature of the source must match the signature of the destination
        /// This allows reuse of abstractions without having to include the interface definition 
        /// This type of wiring is also sutable where there is no implicit interface e.g. the source signature and detination signature do not match
        /// In such cases an adapter pattern is used. The application creates an adpater that matches the signature of the source and contains a port that matches the signature of the destination.
        /// Then it can use this method to wire many abstractions or instances to an instance of the adapter and manually wire the adapter to the destination.
        /// </summary>
        /// <param name="nameSpace"></param>
        /// <param name="fieldOrPropertyName"></param>
        /// <param name="destinationObject"></param>
        /// <param name="destinationMethodName"></param>
        public void WireManyPortsTo(string nameSpace, string fieldOrPropertyName, object destinationObject, string destinationMethodName)
        {
            var classes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => String.Equals(t.Namespace, nameSpace, StringComparison.Ordinal));

            foreach (var cl in classes)
            {
                // diagnosticOutput($"Type {cl.Name}");
                var propertyInfo = cl.GetProperty(fieldOrPropertyName);
                if (propertyInfo != null)
                {
                    Type tDelegate = propertyInfo.PropertyType;
                    // TBD figure out how to make this work if the source is an event 
                    Delegate d = Delegate.CreateDelegate(tDelegate, destinationObject, destinationMethodName);
                    // TBD figure out how to += to the delegate or event so it can be wired more than once
                    propertyInfo.SetValue(null, d);
                    diagnosticOutput($"Wired {cl.Name}.{propertyInfo.Name} to {destinationObject.GetType().Name}.{destinationMethodName}");
                }
                else
                {
                    var fieldInfo = cl.GetField(fieldOrPropertyName);  
                    if (fieldInfo != null)
                    {
                        Type tDelegate = fieldInfo.FieldType;
                        Delegate d = Delegate.CreateDelegate(tDelegate, destinationObject, destinationMethodName);
                        fieldInfo.SetValue(null, d);
                        diagnosticOutput($"Wired {cl.Name}.{fieldInfo.Name} to {destinationObject.GetType().Name}.{destinationMethodName}");
                    }
                }
            }

        }


        /// TBD firgure out how to find all the objects of a class needed to complete the above two methods
        /// This is not supported by reflection because it apparently violates privacy. (Well all reflection can do that - not sure I understand the inconsistency.)
        /// What we are trying to do is analogous to aspects - an example cross-utting concern is logging, but we don't want to create a dependency on logging,
        /// we want to use a port, but there are many of them to wire so we want to mass wire them.
        /// 
        /// To work around this, every abstraction that wants to allow its instances to be individually wired or mass wired will need to have, in their constructor,
        /// code to add an object reference to a static list of objects.
        /// Then these methods can use reflection to see if that static list is present.
        /// In the meantime, only static ports are supported for mass wiring.


        // output port for logging or debugging from all instances of this abstraction
        public delegate void DiagnosticOutputDelegate(string output);
        private static DiagnosticOutputDelegate diagnosticOutput;
        public static DiagnosticOutputDelegate DiagnosticOutput { get => diagnosticOutput; set => diagnosticOutput = value; }

    }
}
