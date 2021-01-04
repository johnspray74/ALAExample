﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Libraries;
using ProgrammingParadigms;

namespace DomainAbstractions
{
    /// <summary>
    /// <para>Adds elements to a list one by one, and outputs the entire list either on an IEvent or once the list reaches a certain output length.</para>
    /// <para>Ports:</para>
    /// <para>1. IDataFlow&lt;T&gt; element: An element to be added to the end of the collection.</para>
    /// <para>2. IEvent clearList: clears the list and (optionally) outputs the data.</para>
    /// <para>3. IDataFlow&lt;List&lt;T&gt;&gt; listOutput: the collection as a List&lt;T&gt;.</para>
    /// </summary>
    public class Collection<T> : IDataFlow<T>, IEvent
    {
        // Properties
        public string InstanceName = "Default";
        public int OutputLength = -1; // Output when the list reaches this length, -1 for output at any length, -2 to disable auto-output
        public bool OutputOnEvent = false;
        public bool ClearOnOutput = false;

        // Private fields
        private List<T> list = new List<T>();

        // Ports
        private IDataFlow<List<T>> listOutput;

        /// <summary>
        /// <para>Adds elements to a list one by one, and outputs the entire list either on an IEvent or once the list reaches a certain output length.</para>
        /// </summary>
        public Collection() { }

        // IDataFlow<T> implementation
        T IDataFlow<T>.Data
        {
            set
            {
                var valueToAdd = value;
                list.Add(valueToAdd);
                list.LogDataChange();
                if ((OutputLength == -1 || list.Count == OutputLength) && listOutput != null)
                {
                    var listCopy = list.Select(s => s).ToList();
                    if (ClearOnOutput) list.Clear();
                    listOutput.Data = listCopy;
                }
            }
        }

        // IEvent implementation
        void IEvent.Execute()
        {
            var listCopy = list.Select(s => s).ToList();
            list.Clear();
            if (listOutput != null && OutputOnEvent) listOutput.Data = listCopy;
        }
    }
}
