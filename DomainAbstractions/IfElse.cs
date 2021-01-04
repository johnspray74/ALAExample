using ProgrammingParadigms;

namespace DomainAbstractions
{
    /// <summary>
    /// Fires an output event to one of two destinations based on whether the condition is true or false.
    /// The process begins either on an IEvent or when it receives an IDataFlow<bool>.
    /// The condition's value can also be set as a public property.
    /// ------------------------------------------------------------------------------------------------------------------
    /// Ports:
    /// 1. IEvent start: Signals to start the process.
    /// 2. IDataFlow<bool> condition: The condition to check. On receiving this value, the process will start.
    /// 3. IEvent ifOutput: The IEvent destination if the condition is true.
    /// 4. IEvent elseOutput: The IEvent destination if the condition is false.
    /// </summary>
    public class IfElse : IEvent, IDataFlow<bool> // start, condition
    {
        // Properties
        public string InstanceName = "Default";
        public bool Condition { set; get; }

        // Ports
        private IEvent ifOutput;
        private IEvent elseOutput;

        // private fields
        private bool executeOnDataflow;

        public IfElse(bool executeOnDataflow = true, bool defaultCondition = true)
        {
            this.executeOnDataflow = executeOnDataflow;
            Condition = defaultCondition;
        }

        public void ExecuteConditional()
        {
            if (Condition)
            {
                ifOutput?.Execute();
            }
            else
            {
                elseOutput?.Execute();
            }
        }

        // IEvent implementation
        void IEvent.Execute()
        {
            ExecuteConditional();
        }

        // IDataFlow<bool> implementation
        bool IDataFlow<bool>.Data
        {
            set
            {
                Condition = value;
                if (executeOnDataflow) ExecuteConditional();
            }
        }
    }
}
