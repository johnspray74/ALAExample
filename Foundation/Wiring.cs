using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.IO;

namespace Foundation
{
    public static class Wiring
    {
        public delegate void DiagnosticOutputDelegate(string output);
        private static DiagnosticOutputDelegate diagnosticOutput;
        public static DiagnosticOutputDelegate DiagnosticOutput { get => diagnosticOutput; set => diagnosticOutput = value; }

        private delegate void InitializeDelegate();
        private static event InitializeDelegate Initialize;

        public delegate void PostWiringInitializeDelegate();
        private static string firstPortName;


        /// <Summary>
        /// wireTo is an extension method on the type object
        /// Important method that wires and connects instances of classes that have ports by matching interfaces (with optional port names).
        /// If object A (this) has a private field of an interface, and object B implements the interface, then wire them together using reflection.
        /// Returns this for fluent style programming.
        /// </summary>
        /// ------------------------------------------------------------------------------------------------------------------
        /// WireTo method understanding what it doess:
        /// <param name="A">
        /// The object on which the method is called is the object being wired from. It must have a private field of the interface type.
        /// </param> 
        /// <param name="B">The object being wired to. It must implement the interface)</param> 
        /// <returns>this to support fluent programming style which allows multiple wiring to the same A object with dot operators</returns>
        /// <remarks>
        /// 1. only wires compatible interfaces, A uses the interface and B implements the interface
        /// 2. interface field must be private (this prevents confusion when using the abstraction of seeing a public field) 
        /// 3. can only wire a single matching interface per call (wires the first one it finds in class A starting at the top of the class)
        /// 4. skips ports in class A that are already wired
        /// 5. you can overide the above order by specifying port names as a second parameter to the wireTo method
        /// 6. looks for list as well (be careful of a list of interface blocking other fields of the same interfaces type lower down from ever being wired)
        /// ------------------------------------------------------------------------------------------------------------------
        /// Also looks for a method: "private void PostWiringInitialize()" in either or both objects being wired.
        /// This method is added to a list of such methods, and all are called once when our method "public static void PostWiringInitialize()" is called (which should be called immediately after all wiring code has executed).
        /// PostWiringInitialize() methods are used to complete any initializaation that needs to be done inside a domain abstraction instance after all the wiring has been completed but before the application begins running.
        /// [Note that PostWiringInitialize methods in different domain abstraction instances are called in a uncontrolled order, so only do things that don't interact with other instances.
        /// Intialization that needs to be done in an ordered fashion: suggest they should be done using Initialize ports on domain abstractions.
        /// Instances of these can then be explicitly wired in the application diagram to a chain of IEvent connectors (Activity diagram programming paradigm).
        /// Alternatively these domain abstractions could have an Initialize input port and an Initialize output port which can then be wired in a chain (the approach of function blocks)]
        /// ------------------------------------------------------------------------------------------------------------------
        /// Whenever a port is wired on the A side, it looks for a private method with the name of the port and ending with the word "Initialize" and calls that method immediately after doing the wiring.
        /// For example if a port exists "private IEvent output", and a method exists "private void outputInitialize()" then outputInitialize gets called after output has been assigned to its wired object B.
        /// These methods are typically used in the domain abstraction to attach a C# event handler to any C# events that are in programming paradigm interface that has just been wired to.
        /// ------------------------------------------------------------------------------------------------------------------
        /// If A has two private fields of the same interface, the first compatible B object wired goes to the first one and the second compatible B object wired goes to the second.
        /// If A has multiple private interfaces of different types, only the first matching interface that B implements will be wired.
        /// By default, only one interface is wired between A and B
        /// To override this behaviour you can get give multiple interfaces in A a prefix "Pn_" where n is 0..9:
        /// Then all fields with the same prfix are considered to be a single logical port.
        /// Then a single wiring operation will wire all fields with a consistent port prefix to the same B object.
        /// These remarks apply only to single fields, not Lists.
        /// e.g.
        /// private IX client1X;
        /// private IY client1Y;
        /// private IZ client2;
        /// Here we want to wire the first two fields to the same object.
        /// So name the fields like this:
        /// private IX P1_client1X;
        /// private IY P1_client1Y;
        /// private IZ client2;
        /// </remarks>
        /// Future owrk: Should WireTo be an extension method on an object called "DomainAbstraction" just so it doesn't become available to call on all other normal objects?
        /// This would mean that all DomainAbstraction classes must inherit from the completely empty abstract class "DomainAbstraction", which wouldn't be so bad.
        public static T WireTo<T>(this T A, object B, string APortName = null, bool reverse = false)
        {
            string multiportExceptionMessage = $"The following wiring failed because the two instances are already wired together by another port.";
            multiportExceptionMessage += "\nPlease use a new WireTo with this additional port name specified:";

            // Get the two instance name first for the Debug Output WriteLines
            var AinstanceName = A?.GetType().GetProperties().FirstOrDefault(f => f.Name == "InstanceName")?.GetValue(A);
            if (AinstanceName == null) AinstanceName = A?.GetType().GetFields().FirstOrDefault(f => f.Name == "InstanceName")?.GetValue(A);
            var BinstanceName = B?.GetType().GetProperties().FirstOrDefault(f => f.Name == "InstanceName")?.GetValue(B);
            if (BinstanceName == null) BinstanceName = B?.GetType().GetFields().FirstOrDefault(f => f.Name == "InstanceName")?.GetValue(B);

            string DiagnosticLine = $"Failed to wire {A?.GetType().Name}[{AinstanceName}].{APortName} to {B?.GetType().Name}[{BinstanceName}]";

            if (A == null)
            {
                throw new ArgumentException("A cannot be null " + DiagnosticLine);
            }
            if (B == null)
            {
                throw new ArgumentException("B cannot be null " + DiagnosticLine);
            }



            // achieve the following via reflection
            // A.field = (<type of interface>)B;
            // A.list.Add( (<type of interface>)B );


            var BType = B.GetType();
            var AfieldInfos = A.GetType().GetFields(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .Where(f => (APortName == null || f.Name == APortName) && (!reverse ^ EndsIn_B(f.Name))).ToList(); // find the fields that the name meets all criteria
                                                                                                                   // TODO: when not reverse ports ending in _B should be excluded

            var wiredSomething = false;
            firstPortName = null;
            foreach (var BimplementedInterface in BType.GetInterfaces()) // consider every interface implemented by B 
            {
                var AfieldInfo = AfieldInfos.FirstOrDefault(f => f.FieldType == BimplementedInterface && f.GetValue(A) == null); // find the first field in A that matches the interface type of B
                                                                                                                                 // TODO: the list case below should have the SamePort constraint as well
                
                // look for normal private fields first
                if (AfieldInfo != null)  // there is a match
                {
                    
                    if (SamePort(AfieldInfo.Name))
                    {
                        if (wiredSomething)
                        {
                            string exceptionMessage = multiportExceptionMessage + "\n" + WiringToString(A, B, AfieldInfo);
                            // throw new Exception(exceptionMessage);
                        }
                            
                        AfieldInfo.SetValue(A, B);  // do the wiring
                        wiredSomething = true;

                        // see if there is a PostWiring function associated with the port and call it.
                        var m = A.GetType().GetMethod($"{AfieldInfo.Name}Initialize", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                        if (m != null)
                        {
                            PostWiringInitializeDelegate handler = (PostWiringInitializeDelegate)Delegate.CreateDelegate(typeof(PostWiringInitializeDelegate), A, m);
                            handler();
                        }

                        DiagnosticOutput?.Invoke(WiringToString(A, B, AfieldInfo));
                    }
                    continue;  // could be more than one interface to wire
                }

                // do the same as above for private fields that are a list of the interface of the matching type
                foreach (var AlistFieldInfo in AfieldInfos)
                {
                    if (!AlistFieldInfo.FieldType.IsGenericType) //not matching interface
                    {
                        continue;
                    }
                    var AListFieldValue = AlistFieldInfo.GetValue(A);

                    var AListGenericArguments = AlistFieldInfo.FieldType.GetGenericArguments();
                    if (AListGenericArguments.Length != 1) continue;    // A list should only have one type anyway 
                    if (AListGenericArguments[0].IsAssignableFrom(BimplementedInterface)) // JRS: There was some case where == didn't work, maybe in the gamescoring application
                    {
                        if (AListGenericArguments[0] != BimplementedInterface)
                        {
                            var g = AListGenericArguments[0];
                            //if (g != typeof(object)) throw new Exception($"Different types {g} {AListGenericArguments[0]} {typeof(object)}");
                            continue;
                        }
                        if (AListFieldValue == null)
                        {
                            var listType = typeof(List<>);
                            Type[] listParam = { BimplementedInterface };
                            AListFieldValue = Activator.CreateInstance(listType.MakeGenericType(listParam));
                            if (wiredSomething)
                            {

                                string exceptionMessage = multiportExceptionMessage + "\n" + WiringToString(A, B, AListFieldValue);
                                throw new Exception(exceptionMessage);
                            }
                                
                            AlistFieldInfo.SetValue(A, AListFieldValue);
                        }

                        AListFieldValue.GetType().GetMethod("Add").Invoke(AListFieldValue, new[] { B });
                        wiredSomething = true;

                        // see if there is a PostWiringInitialize function associated with the port and call it.
                        var m = A.GetType().GetMethod($"{AlistFieldInfo.Name}Initialize", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                        if (m != null)
                        {
                            PostWiringInitializeDelegate handler = (PostWiringInitializeDelegate)Delegate.CreateDelegate(typeof(PostWiringInitializeDelegate), A, m);
                            handler();
                        }

                        DiagnosticOutput?.Invoke(WiringToString(A, B, AlistFieldInfo));
                        break;
                    }

                }
            }

            if (!reverse && !wiredSomething)
            {
                if (APortName != null)
                {
                    // a specific port was specified so see if the port was already wired
                    var AfieldInfo = AfieldInfos.FirstOrDefault();
                    if (AfieldInfo?.GetValue(A) != null) throw new Exception($"Port already wired {A.GetType().Name}[{AinstanceName}].{APortName} to {BType.Name}[{BinstanceName}]");
                }
                throw new Exception($"Failed to wire {A.GetType().Name}[{AinstanceName}].{APortName} to {BType.Name}[{BinstanceName}]");
            }

            var method = A.GetType().GetMethod("PostWiringInitialize", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (method != null)
            {
                InitializeDelegate handler = (InitializeDelegate)Delegate.CreateDelegate(typeof(InitializeDelegate), A, method);
                Initialize -= handler;  // instances can be wired to/from more than once, so only register their PostWiringInitialize once
                Initialize += handler;
            }
            /*
            method = B.GetType().GetMethod("PostWiringInitialize", System.Reflection.BindingFlags.NonPublic);
            if (method != null)
            {
                InitializeDelegate handler = (InitializeDelegate)Delegate.CreateDelegate(typeof(InitializeDelegate), B, method);
                Initialize += handler;
            }
            */
            return A;
        }

        /// <summary>
        /// Wire B to A and returns A. Used to wire objects to input ports of A.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="A">The object being wire to</param>
        /// <param name="B">The object being wired from</param>
        /// <returns>A</returns>
        public static object WireFrom<T>(this object A, T B, string APortName = null)
        {
            B.WireTo(A, APortName);
            return A;
        }


        /// <summary>
        /// The SamePort function always returns true the first time it is called (for a given A and B) but on the second and subsequent calls
        /// it only returns true if the name has the same Px_ prefix as the first.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private static bool SamePort(string name)
        {
            if (name.Length >= 3 && name[0] == 'P' && name[2] == '_' && name[1] >= '0' && name[1] <= '9')
            {
                string portName = name.Substring(0, 3);
                if (firstPortName == null)
                {
                    firstPortName = portName;
                }
                return portName.Equals(firstPortName);
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private static bool EndsIn_B(string s)
        {
            if (s == null) return false;
            var rv = s.Length > 2 && s.EndsWith("_B");
            if (rv)
            {
            }
            return s.Length > 2 && s.EndsWith("_B");
        }




        public static T IfWireTo<T>(this T A, bool condition, object B, string portName = null)
        {
            if (condition) A.WireTo(B, APortName: portName);
            return A;
        }

        public static T IfWireFrom<T>(this T A, bool condition, object B, string portName = null)
        {
            if (condition) A.WireFrom(B, APortName: portName);
            return A;
        }

        public static void PostWiringInitialize()
        {
            Initialize?.Invoke();
        }



        private static string WiringToString(dynamic A, dynamic B, dynamic matchedInterface)
        {
            string AInstanceName = "No InstanceName";
            string BInstanceName = "No InstanceName";
            try { if (A.InstanceName != null) AInstanceName = $"{A.InstanceName}"; } catch { };
            try { if (B.InstanceName != null) BInstanceName = $"{B.InstanceName}"; } catch { };

            var AClassName = A.GetType().Name;
            var BClassName = B.GetType().Name;
            string matchedInterfaceType = $"{matchedInterface.FieldType.Name}";
            if (matchedInterface.FieldType.GenericTypeArguments.Length > 0)
            {
                matchedInterfaceType += $"<{matchedInterface.FieldType.GenericTypeArguments[0]}>";
            }

            return $"WireTo {AClassName}[{AInstanceName}].{matchedInterface.Name} <{matchedInterfaceType}> ---> {BClassName}[{BInstanceName}]";
        }
 
    }
}
