using System.Collections.Generic;
using Foundation;

namespace ProgrammingParadigms
{
    /// <summary>
    /// A dataflow with a single scaler value with a primitive data type and a "OnChanged' event.
    /// OR think of it as an event with data, the receivers are able to read the data at any time.
    /// Or think of it as an implementation of a global variable and an observer pattern, with access to the variable and observer pattern restricted to the line connections on the diagram.
    /// Unidirectional - every line is one direction implying sender(s) and receiver(s).
    /// You can have multiple senders and receivers.
    /// The data is stored in the wire so receivers that don't act on the event can read its value at any time. Receivers cant change the data or send the event. 
    /// </summary>
    /// <typeparam name="T">Generic data type</typeparam>
    public interface IDataFlow<T>
    {
        void Push(T data);
    }

    /// <summary>
    /// A reversed IDataFlow, the IDataFlow_R uses an event internally to allow the wiring to be reversed.
    /// Required when a domain abstraction needs multiple input ports of the same type.
    /// Wiring requires use of DataFlowConenctor
    /// </summary>
    /// <typeparam name="T">Generic data type</typeparam>

    public delegate void PushDelegate<T>(T data);
    public interface IDataFlow_R<T>
    {
        event PushDelegate<T> Push;
    }

    /// <summary>
    /// Acts similar to a connector object in a components and connectors architecture
    /// Usually it is not necessary to wire two instances of a domain abstraction with this connector object - you can wire two complimentary IDataFlow ports directly.
    /// It is only needed for situations where you want fanout, or to use IDataFlow_R, or to chain connectors to control order of execution
    /// (or to connect a push to a pull port. IDataFlowPull not yet implemented)
    /// Note IDataflow_R is wired in the reverse direction of dataflow (from the destination to the source). It's purpose is to allow a domain abstractions to have more than one input of the same type (becasue you can't implement IDataFlow twice for the same type)
    /// ----------------------------------------------------------------------------------------------
    /// Ports:
    /// 1. IDataFlow<T> IDataFlow<T>: incoming data port
    /// 2. IDataFlow_R<T> IDataFlow_R<T>: the reverse wired output port. Note: this output happens after the fanoutList but before last
    /// 3. List<IDataFlow<T>> fanoutList: output port that fans out to every abstraction connected in order of wiring
    /// 4. IDataFlow<T> last: output port that will output after the fanoutList and the IDataFlow_B. This enables chaining of these connectors to explicitly control order of execution of wired outputs.
    /// </summary>
    /// <typeparam name="T">Generic data type</typeparam>
    public class DataFlowConnector<T> : IDataFlow<T>, IDataFlow_R<T>  // Input, Output
    {
        // properties
        public string InstanceName = "Default";

        // ports
        private List<IDataFlow<T>> fanoutList = new List<IDataFlow<T>>();
        private IDataFlow<T> last;

        /// <summary>
        /// Fans out a data flow to mutiple data flows, or connect IDataFlow and IDataFlow_B
        /// </summary>
        public DataFlowConnector() { }


        // IDataFlow<T> implementation ---------------------------------

        void IDataFlow<T>.Push(T data)
        {
            foreach (var f in fanoutList) f.Push(data);
            Push?.Invoke(data);
            if (last != null) last.Push(data);
        }

        // IDataFlow_R<T> implementation ---------------------------------
        private event PushDelegate<T> Push;
        event PushDelegate<T> IDataFlow_R<T>.Push { add { Push += value; } remove { Push -= value; } }
    }
}
