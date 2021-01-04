using ProgrammingParadigms;

namespace DomainAbstractions
{
    /// <summary>
    /// Works similar as the object.Equals() method. It compares a stored value with an input value, and emits an IDataFlow<bool> to indicate the comparison result.
    /// Used frequently for recognising connected device names. When using this abstraction, constructor must have the data you want to compare with e.g. new Equals<string>("SRS2")
    /// ------------------------------------------------------------------------------------------------------------------
    /// Ports:
    /// 1. IDataFlow<T> dataToBeCompared: input generic data that wants to be compared with to the internal stored data that was defined on construction of the specific Equals abstraction object
    /// 2. IDataFlow<bool> isEqual: output boolean result of whether the input port data is 'equal' to the stored value
    /// 3. IEvent equalEvent: fired when input is equal to the stored value
    /// </summary>
    /// <typeparam name="T">Generic Type of any kind of comparing data</typeparam>
    public class Equals<T> : IDataFlow<T> // dataToBeCompared
    {
        // properties ---------------------------------------------------------------
        public string InstanceName = "Default";

        // ports ---------------------------------------------------------------
        private IDataFlow<bool> isEqual;
        private IEvent equalEvent;

        // private fields ---------------------------------------------------------------
        private T compareData;
        private bool equal;

        /// <summary>
        /// Works similar as the object.Equals() method. 
        /// </summary>
        public Equals(T compareData)
        {
            this.compareData = compareData;
        }

        // IDataFlow<T> implmentation ----------------------------------------------------------------------------------------
        T IDataFlow<T>.Data
        {
            set
            {
                if (compareData == null)
                {
                    equal = (null == value);
                }
                else
                {
                    equal = compareData.Equals(value);
                }

                if (isEqual != null)
                {
                    isEqual.Data = equal;
                }

                if (equal) equalEvent?.Execute();
            }
        }
    }
}
