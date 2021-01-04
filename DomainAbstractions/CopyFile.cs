using ProgrammingParadigms;
using System;
using System.IO;

namespace DomainAbstractions
{
    /// <summary>
    /// Copies a given file to a given destination when triggered by an IEvent.
    /// The source and destination paths can be given via the constructor and/or
    /// IDataFlowB<string> ports.
    /// ------------------------------------------------------------------------
    /// Ports:
    /// 1. IEvent NEEDNAME: Starts the copy process.    If source and/or dest are not present,
    ///                                                 it will wait until both are present then perform
    ///                                                 the copy.
    /// 2. IDataFlowB<string> sourcePathPort:           The source path to copy from.
    /// 3. IDataFlowB<string> destPathPort:             The destination to copy the file to.
    /// 4. IEvent copySuccess:                          Fired when the copy succeeds.
    /// 5. IEvent copyFailed:                           Fired when the copy fails.
    /// 6. IDataFlow<string> failReason:                The reason the copy failed.
    /// </summary>
    class CopyFile : IEvent
    {
        // properties
        public string InstanceName = "Default";

        // ports
        private IDataFlowB<string> sourcePathPort;
        private IDataFlowB<string> destPathPort;
        private IEvent copySuccess;
        private IEvent copyFailed;
        private IDataFlow<string> failReason;

        // private fields
        private string sourcePath;
        private string destPath;
        private bool overwrite;
        private bool eventHasFired = false;

        /// <summary>
        /// Copies a given file to a given destination when triggered by an IEvent.
        /// The source and destination paths can be given via the constructor and/or
        /// IDataFlowB<string> ports.
        /// </summary>
        /// <param name="sourcePath">The source path to copy from.</param>
        /// <param name="destPath">The destination to copy the file to.</param>
        /// <param name="overwrite">Whether the destination should be overwritten if it already exists.</param>
        public CopyFile(string sourcePath = default, string destPath = default, bool overwrite = true)
        {
            this.sourcePath = sourcePath;
            this.destPath = destPath;
            this.overwrite = overwrite;
        }

        /// <summary>
        /// Sets up the listeners for the source and destination path ports.
        /// </summary>
        private void PostWiringInitialize()
        {
            if (sourcePathPort != null)
            {
                sourcePathPort.DataChanged += () =>
                {
                    sourcePath = sourcePathPort.Data;
                    if (eventHasFired) PerformCopy();
                };
            }

            if (destPathPort != null)
            {
                destPathPort.DataChanged += () =>
                {
                    destPath = destPathPort.Data;
                    if (eventHasFired) PerformCopy();
                };
            }
        }

        /// <summary>
        /// Performs the copy operation.
        /// </summary>
        private void PerformCopy()
        {
            if (sourcePath == default || destPath == default) return;
            eventHasFired = false;

            try
            {
                File.Copy(sourcePath, destPath, overwrite);
                copySuccess?.Execute();
            }
            catch(Exception e)
            {
                if (failReason != null) failReason.Data = e.Message;
                copyFailed?.Execute();
            }
        }

        // IEvent implementation
        void IEvent.Execute()
        {
            eventHasFired = true;
            PerformCopy();
        }
    }
}
