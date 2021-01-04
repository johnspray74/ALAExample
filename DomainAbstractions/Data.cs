﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgrammingParadigms;
using Libraries;

namespace DomainAbstractions
{
    /// <summary>
    /// <para>Stores any data of any type, and outputs that data when it receives an IEvent call. Also supports limiting the output to a certain number of times.</para>
    /// <para>Ports:</para>
    /// <para>1. IEvent start: Starts the process of sending out the currently stored data.</para>
    /// <para>2. IDataFlow&lt;T&gt; inputData: The data to store.</para>
    /// <para>2. IDataFlowB&lt;T&gt; returnData: Returns the currently stored data.</para>
    /// <para>2. IDataFlowB&lt;T&gt; inputDataB: A source to pull data from. This data replaces the currently stored data before the data output occurs.</para>
    /// <para>2. IDataFlow&lt;T&gt; dataOutput: The destination to send the currently stored data.</para>
    /// </summary>
    public class Data<T> : IEvent,  IDataFlow<T>, IDataFlowB<T>
    {
        // Properties
        public string InstanceName = "Default";
        public bool Perishable = false;
        public int PerishCount = 1; // Data cannot be pushed or pulled after it has been sent this amount of times

        // Public fields
        public T storedData = default(T);

        // Private fields
        private int numTimesSent = 0;

        // Ports
        private IDataFlowB<T> inputDataB;
        private IDataFlow<T> dataOutput;

        /// <summary>
        /// <para>Stores any data of any type, and outputs that data when it receives an IEvent call. Also supports limiting the output to a certain number of times.</para>
        /// </summary>
        public Data()
        {
            // Test();
        }

        private void Test()
        {
            
        }

        // IEvent implementation
        void IEvent.Execute()
        {
            numTimesSent++;
            if (dataOutput != null)
            {
                // dataOutput.LogDataChange($"Data<{typeof(T)}> {InstanceName}", "dataOutput", , storedData);
                if (inputDataB != null) storedData = inputDataB.Data;
                storedData.LogDataChange($"IDataFlow<{typeof(T)}>");
                dataOutput.Data = storedData;
            }
        }

        // IDataFlow<T> implementation
        T IDataFlow<T>.Data
        {
            set
            {
                storedData.LogDataChange($"IDataFlow<{typeof(T)}>");
                storedData = value;
            }
        }

        // IDataFlowB<T> implementation
        public event DataChangedDelegate DataChanged;
        T IDataFlowB<T>.Data
        {
            get
            {
                if (!(Perishable && numTimesSent >= PerishCount))
                {
                    numTimesSent++;
                    return storedData;
                }
                else
                {
                    return default(T);
                }
            }
        }
    }
}
