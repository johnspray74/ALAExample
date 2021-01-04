using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgrammingParadigms;

namespace DomainAbstractions
{
    /// <summary>
    /// [UNUSED]
    /// Listens to data from any number of inputs and pushes data whenever their data changes.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Listener<T> : IDataFlowB<T>
    {
        // Properties
        public string InstanceName = "Default";

        // Private fields
        private T data = default;

        // Ports
        private List<IDataFlowB<T>> listenTo;
        private List<IDataFlow<T>> fanoutList;

        public Listener() { }

        // IDataFlowB<T> implementation
        public event DataChangedDelegate DataChanged;

        T IDataFlowB<T>.Data { get => data; }

        private void PostWiringInitialize()
        {
            foreach (var input in listenTo)
            {
                input.DataChanged += () =>
                {
                    data = input.Data;
                    if (fanoutList != null)
                    {
                        foreach (var output in fanoutList)
                        {
                            output.Data = data;
                        } 
                    }
                    DataChanged?.Invoke();
                };
            }
        }
    }
}
