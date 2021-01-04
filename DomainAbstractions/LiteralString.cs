using ProgrammingParadigms;

namespace DomainAbstractions
{
    /// <summary>
    /// Using StringFormat with nothing connected to the list of inputs
    /// ------------------------------------------------------------------------------------------------------------------
    /// Ports:
    /// 1. IDataFlow<bool> "NEEDNAME": input boolean to determine whether to push the literal string to the output dataFlowOutput port
    /// 2. IEvent "NEEDNAME": Event to also push the literal string to the output dataFlowOutput port
    /// 3. IDataFlow<string> dataFlowOutput: string output
    /// </summary>
    public class LiteralString : IDataFlow<bool>, IEvent
    {
        // properties -----------------------------------------------------------------
        public string InstanceName = "Default";

        // ports -----------------------------------------------------------------
        private IDataFlow<string> dataFlowOutput;

        // private fields
        private string literalString;

        /// <summary>
        /// Using StringFormat with nothing connected to the list of inputs
        /// </summary>
        /// <param name="liter">the literal string</param>
        public LiteralString(string liter)
        {
            literalString = liter;
        }

        // IDataFlow<bool> implementation -------------------------------
        bool IDataFlow<bool>.Data { set { if (value) dataFlowOutput.Data = literalString; } }

        // IEvent implementation --------------------------------------
        void IEvent.Execute() => dataFlowOutput.Data = literalString;
    }
}
