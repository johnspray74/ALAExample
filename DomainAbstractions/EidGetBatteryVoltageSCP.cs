using ProgrammingParadigms;
using System;

namespace DomainAbstractions
{
    /// <summary>
    /// Checks the voltage of an EID device.
    /// -----------------------------------------------------------------------------------------------
    /// Ports:
    /// 1. IEvent "NEEDNAME": To be called when we need to get the battery voltage.
    /// 2. IDataFlow<double> batteryVoltage: The voltage read from the EID, in volts (V).
    /// </summary>
    class EidGetBatteryVoltageSCP : IEvent
    {
        // properties
        public string InstanceName = "Default";

        // ports
        private IRequestResponseDataFlow_B<string, string> requestResponseDataFlow;
        private IArbitrator arbitrator;
        private IDataFlow<double> batteryVoltage;

        /// <summary>
        /// Gets the voltage of an EID device.
        /// </summary>
        public EidGetBatteryVoltageSCP() { }

        // IEvent implementation -----------------------------------------------------------------
        // Normally, this mehod will not be declared with "async", the reason using "async" is that 
        // it calls other method with "await", which requires an "async" key word to decorate the method
        async void IEvent.Execute()
        {
            double voltage = 0;

            // Retrieve voltage from the EID
            await arbitrator.Request(InstanceName);
            string rawVoltage = await requestResponseDataFlow.SendRequest("{BV}");
            arbitrator.Release(InstanceName);

            try
            {
                rawVoltage = rawVoltage.Replace("V", "").Replace("(CH)", "").Replace(" ", ""); // remove voltage unit, charging indicator and space
                voltage = Convert.ToDouble(rawVoltage); // cast to double
            }
            // read value is not castable, return 0 voltage
            catch (Exception)
            {}

            batteryVoltage.Data = voltage;
        }
    }
}