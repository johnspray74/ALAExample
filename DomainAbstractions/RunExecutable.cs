using ProgrammingParadigms;
using System.Diagnostics;

namespace DomainAbstractions
{
    /// <summary>
    /// Starts an executable. Calls either a success or failure event
    /// depending on the status of the executable.
    /// -------------------------------------------------------------
    /// Ports:
    /// 1. IEvent NEEDNAME:             Starts the process.
    /// 2. IDataFlow<string> NEEDNAME:  The path to the executable.
    /// 3. IEvent running:              Fired when the process has been started.
    /// 4. IEvent failedToStart:        Fired when the process failed to start.
    /// </summary>
    class RunExecutable : IEvent, IDataFlow<string>
    {
        // properties
        public string InstanceName = "Default";

        // ports
        private IEvent running;
        private IEvent failedToStart;

        // private fields
        private bool eventHappened = false;
        private string executableLocation;

        /// <summary>
        /// Starts an executable. Calls either a success or failure event
        /// depending on the status of the executable.
        /// </summary>
        /// <param name="executableLocation">The path to the executable.</param>
        public RunExecutable(string executableLocation = default)
        {
            this.executableLocation = executableLocation;
        }

        /// <summary>
        /// Starts the process.
        /// </summary>
        private void Run()
        {
            if (executableLocation == default)
            {
                eventHappened = true;
                return;
            }

            eventHappened = false;

            // start the executable
            try
            {
                Process.Start(executableLocation);
            }
            catch
            {
                failedToStart?.Execute();
                return;
            }

            running?.Execute();
        }

        // IEvent implementation
        void IEvent.Execute() => Run();

        // IDataFlow<string> implementation
        string IDataFlow<string>.Data
        {
            set
            {
                executableLocation = value;
                if (eventHappened) Run();
            }
        }
    }
}
