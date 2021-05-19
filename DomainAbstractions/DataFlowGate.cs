using ProgrammingParadigms;

namespace DomainAbstractions
{
    /// <summary>
    /// A Gate used to block data flow. Work similar to an EventGate, when data flow coming in, it will be stored if the gate
    /// is turned off. Otherwise, the data will be pushed through the dataOutput port.
    /// ------------------------------------------------------------------------------------------------------------------
    /// Ports:
    /// 1. IDataFlow<T> dataToBePushed: generic type of data flow wanting to be pushed
    /// 2. IEvent pushDataRegardlessOfLatch: regardless of whether the gate is closed or open, push the data if it has arrived
    /// 3. IDataFlow<T> dataOutput: generic type of data flow being outputted
    /// 4. IDataFlow_B<bool> triggerLatchInput: is used to control to turn the gate on or off with a boolean value (turned off is false).
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DataFlowGate<T> : IDataFlow<T>, IEvent // dataToBePushed, pushDataRegardlessOfLatch
    {
        // properties ---------------------------------------------------------------
        public string InstanceName = "Default";
        public bool LatchInput = false;

        // ports ---------------------------------------------------------------
        private IDataFlow<T> dataOutput;
        private IDataFlow_B<bool> triggerLatchInput;

        /// <summary>
        /// Control the latch of the gate to block or push the incoming data.
        /// </summary>
        public DataFlowGate() { }

        // IDataFlow<T> implementation ----------------------------------------------------------------------------------------
        private T data;
        T IDataFlow<T>.Data {
            set
            {
                if (LatchInput)
                {
                    dataOutput.Data = value;
                }
                else
                {
                    data = value;
                }
            }
        }

        // IEvent implementation ----------------------------------------------------------------------------------------
        void IEvent.Execute()
        {
            dataOutput.Data = data;
            data = default(T);
        }


        private void PostWiringInitialize()
        {
            if (triggerLatchInput != null)
            {
                triggerLatchInput.DataChanged += () =>
                {
                    LatchInput = triggerLatchInput.Data;
                    if (LatchInput && data != null)
                    {
                        dataOutput.Data = data;
                    }
                };
            }
        }
    }
}
