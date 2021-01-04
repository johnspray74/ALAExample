using ProgrammingParadigms;
using System;

namespace DomainAbstractions
{
    /// <summary>
    /// Abstraction of the C# tuple class.
    /// ----------------------------------
    /// Ports:
    /// 1. IDataFlowB<Tuple<A, B>> getCurrentOutput: get the current tuple output
    /// 2. IDataFlowB<A> portA: input port A.
    /// 3. IDataFlowB<B> portB: input port B.
    /// 4. IDataFlow<Tuple<A, B>> portOut: outputs the tuple when portA or portB are updated.
    /// </summary>
    /// <typeparam name="A">Port A input type.</typeparam>
    /// <typeparam name="B">Port B input type.</typeparam>
    class TupleAbstraction<A, B> : IDataFlowB<Tuple<A, B>> // getCurrentOutput
    {
        // properties
        public string InstanceName = "Default";

        // ports
        private IDataFlowB<A> portA;
        private IDataFlowB<B> portB;
        private IDataFlow<Tuple<A, B>> portOut;

        // private fields
        private Tuple<A, B> output;

        /// <summary>
        /// Abstraction of the C# tuple class.
        /// </summary>
        public TupleAbstraction(A a = default, B b = default)
        {
            output = new Tuple<A, B>(a, b);
        }

        private void PostWiringInitialize()
        {
            DataChangedDelegate del = () =>
            {
                if (portA.Data == null || portB.Data == null) return;
                output = new Tuple<A, B>(portA.Data, portB.Data);
                DataChanged?.Invoke();

                if (portOut != null)
                {
                    portOut.Data = output;
                }
            };

            portA.DataChanged += del;
            portB.DataChanged += del;
        }

        // IDataFlowB implementation
        Tuple<A, B> IDataFlowB<Tuple<A, B>>.Data
        {
            get => output;
        }

        public event DataChangedDelegate DataChanged;
    }
}
