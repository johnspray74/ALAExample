using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgrammingParadigms;

namespace DomainAbstractions
{
    /// <summary>
    /// An event loop that can run continuously in the background using async/await.
    /// Default delay is 1 second and will run infinitely. The delay and times to loop can be changed on initialisation.
    /// ------------------------------------------------------------------------------------------------------------------
    /// Ports:
    /// 1. IEvent input: input event that wants to be delayed</para>
    /// 2. IEvent eventHappened: output event that will be pushed after each delay
    /// </summary>
    public class Timer : IEvent // start
    {
        // properties
        public string InstanceName = "Default";
        public int Delay = 1000;
        public int LoopTimes = -1; // -1 for infinite

        // ports
        private IEvent eventHappened;

        /// <summary>
        /// An event loop that can run continuously in the background using async/await.
        /// </summary>
        public Timer() { }

        private async Task RunDelayedEventLoopAsync(int LoopTimes)
        {
            if (LoopTimes == -1)
            {
                while (true)
                {
                    // Debug.WriteLine($"Event {InstanceName} fired!");
                    if (eventHappened != null) eventHappened.Execute();
                    await Task.Delay(Delay);
                }
            }
            else if (LoopTimes == 1)
            {
                // Debug.WriteLine($"Event {InstanceName} fired!");
                await Task.Delay(Delay);
                if (eventHappened != null) eventHappened.Execute();
            }
            else
            {
                for (int i = 0; i < LoopTimes; i++)
                {
                    // Debug.WriteLine($"Event {InstanceName} fired!");
                    if (eventHappened != null) eventHappened.Execute();
                    await Task.Delay(Delay);
                }
            }

        }

        // IEvent implementation ---------------------------------------------------------
        void IEvent.Execute()
        {

            var _fireAndForget = RunDelayedEventLoopAsync(LoopTimes);

        }
    }
}
