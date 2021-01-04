using ProgrammingParadigms;

namespace DomainAbstractions
{
    /// <summary>
    /// A logic converter, working on IDataFlow<bool>. Converting true to false, or false to true
    /// ------------------------------------------------------------------------------------------------------------------
    /// Ports:
    /// 1. IDataFlow<bool> incomingBool: boolean value wanting to be reversed
    /// 2. IDataFlow<bool> reversedInput: result of the 'Not' operation on the incoming boolean value
    /// </summary>
    public class Not : IDataFlow<bool> // incomingBool
    {
        // properties
        public string InstanceName = "Default";

        // outputs
        IDataFlow<bool> reversedInput;

        /// <summary>
        /// A not logic which converts boolean IDataFlow true to false, or false to true.
        /// </summary>
        public Not() { }

        // IDataFlow<bool> implementation -------------------------------------------------------
        bool IDataFlow<bool>.Data { set => reversedInput.Data = !value; }
    }
}
