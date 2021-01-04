using Libraries;
using ProgrammingParadigms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace DomainAbstractions
{
    /// <summary>
    /// Takes multiple sources of firmware information and combines them into a single dictionary.
    /// ------------------------------------------------------------------------------------------
    /// Ports:
    /// 1. IEvent "NEEDNAME": Resets the version cache.
    /// 2. List<IDataFlowB<SoftwareVersions>> firmwareVersionInputs: List of data flows which will push firmware information.
    /// 3. IDataFlow<Dictionary<string, SoftwareVersionsSoftwaresSoftware>> firmwareVersionsOutput: Output port, pushes a dictionary of software versions
    ///                                                                                             with the key as the software name e.g. Series5000, 
    ///                                                                                             XRS2 and the value as the firmware information.
    /// 4. IEvent dataCollected: Fired when all data sources have responded.
    /// </summary>
    class FirmwareVersionCollector : IEvent
    {
        // properties
        public string InstanceName = "Default";

        // ports
        private List<IDataFlowB<SoftwareVersions>> firmwareVersionInputs = new List<IDataFlowB<SoftwareVersions>>();
        private IDataFlow<Dictionary<string, SoftwareVersionsSoftwaresSoftware>> firmwareVersionsOutput;
        private IEvent dataCollected;

        // private fields
        private Dictionary<string, SoftwareVersionsSoftwaresSoftware> firmwareVersions = new Dictionary<string, SoftwareVersionsSoftwaresSoftware>();
        private Dictionary<int, bool> respondants = new Dictionary<int, bool>();

        /// <summary>
        /// Takes multiple sources of firmware information and combines them into a single dictionary.
        /// </summary>
        public FirmwareVersionCollector() { }

        /// <summary>
        /// Sets up the data changed delegates to retrieve data when it is pushed.
        /// </summary>
        private void PostWiringInitialize()
        {
            for (int i = 0; i < firmwareVersionInputs.Count; i++)
            {
                // c# passes by reference into lambdas, need to create local copy >:(
                int key = i;

                IDataFlowB<SoftwareVersions> input = firmwareVersionInputs[key];
                respondants[key] = false;

                input.DataChanged += () =>
                {
                    // no data yet
                    if (input.Data == null) return;

                    // set the input to responded
                    respondants[key] = true;

                    // add latest values to storage dict
                    foreach (SoftwareVersionsSoftwaresSoftware version in input.Data.Items.First().Software)
                    {
                        firmwareVersions[version.Name] = version;
                    }

                    // check if all sources have responded
                    foreach (int key in respondants.Keys.ToList())
                    {
                        if (!respondants[key]) return;
                    }

                    // send to dataflow
                    if (firmwareVersionsOutput != null) firmwareVersionsOutput.Data = firmwareVersions;
                    dataCollected?.Execute();
                };
            }
        }

        // IEvent implementation
        void IEvent.Execute()
        {
            // clear cache
            firmwareVersions.Clear();

            // reset respondants
            foreach (int key in respondants.Keys.ToList())
            {
                respondants[key] = false;
            }
        }
    }
}
