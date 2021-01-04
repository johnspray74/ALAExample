using Libraries;
using ProgrammingParadigms;
using System;

namespace DomainAbstractions
{
    /// <summary>
    /// Takes a string of current firmware version and an instance of `SoftwareVersionsSoftwaresSoftware`
    /// and compares the versions. The output port will be set to true if the instance is a later version,
    /// otherwise the output port will be set to false.
    /// Both versions must have the same number of periods. Decimal places do not matter.
    /// --------------------------------------------------------------------------------------------------
    /// Ports:
    /// 1. IEvent NEEDNAME: Triggers the compare function.
    /// 2. IDataFlow<SoftwareVersionsSoftwaresSoftware> NEEDNAME: The new version to compare
    /// 3. IDataFlowB<string> currentVersion: The old version to compare
    /// 4. IDataFlowB<SoftwareVersionsSoftwaresSoftware> newVersion: The new version to compare
    /// 5. IDataFlow<bool> isOldVersion: True if the given instance is newer than the current version.
    /// 6. IEvent isOldVersionEvent: Fired if the old version is older than the new version.
    /// 7. IEvent versionMatchEvent: Fired if the versions match.
    /// </summary>
    class CompareFirmwareVersions : IEvent, IDataFlow<SoftwareVersionsSoftwaresSoftware>
    {
        // parameters
        public string InstanceName = "Default";

        // ports
        private IDataFlowB<string> currentVersion;
        private IDataFlowB<SoftwareVersionsSoftwaresSoftware> newVersion;
        private IDataFlow<bool> isOldVersion;
        private IEvent isOldVersionEvent;
        private IEvent versionMatchEvent;

        // private fields
        private bool eventHasFired = false;
        private string currentVersionValue;
        private SoftwareVersionsSoftwaresSoftware newVersionValue = default;

        /// <summary>
        /// Takes a string of current firmware version and an instance of `SoftwareVersionsSoftwaresSoftware`
        /// and compares the versions. The output port will be set to true if the instance is a later version,
        /// otherwise the output port will be set to false.
        /// </summary>
        /// <param name="currentVersionValue">The current version of the firmware.</param>
        public CompareFirmwareVersions(string currentVersionValue = default)
        {
            this.currentVersionValue = currentVersionValue;
        }

        private void PostWiringInitialize()
        {
            // Start with old version = false
            if (isOldVersion != null)
            {
                isOldVersion.Data = false;
            }

            if (currentVersion != null)
            {
                currentVersionValue = currentVersion.Data;
                currentVersion.DataChanged += () =>
                {
                    currentVersionValue = currentVersion.Data;
                    if (eventHasFired) CompareAndUpdate();
                };
            }

            if (newVersion != null)
            {
                newVersionValue = newVersion.Data;
                newVersion.DataChanged += () =>
                {
                    newVersionValue = newVersion.Data;
                    if (eventHasFired) CompareAndUpdate();
                };
            }
        }

        private void CompareAndUpdate()
        {
            // not ready
            if (currentVersionValue == null || newVersionValue == null)
            {
                eventHasFired = true;
                return;
            }

            eventHasFired = false;
            bool result = CompareVersions();

            if (result)
            {
                isOldVersionEvent?.Execute();
            }
            else
            {
                versionMatchEvent?.Execute();
            }

            if (isOldVersion != null)
            {
                isOldVersion.Data = result;
            }
        }

        /// <summary>
        /// Compares two versions of software.
        /// </summary>
        /// <returns>Whether the given version is newer than the current version.</returns>
        private bool CompareVersions()
        {
            string[] oldV = currentVersionValue.Split('.');
            string[] newV = newVersionValue.Version.Split('.');

            // Old and new versions must have the same number of periods
            if (oldV.Length != newV.Length)
            {
                throw new ArgumentException(String.Format("The given versions do not have the same number of periods. Current Version: {0}, New Version: {1}", currentVersion, newVersionValue.Version));
            }

            // Loop through each digit of the firmware version
            for (int i = 0; i < oldV.Length; ++i)
            {
                // Parse the versions into ints
                int old = Int32.Parse(oldV[i]);
                int _new = Int32.Parse(newV[i]);
                
                // If the new version is greater than the old version,
                // it is new. Otherwise, if it is less than the old version,
                // we are already using a newer version. If they are equal,
                // continue to the next digit.
                if (_new > old)
                {
                    return true;
                }
                else if (old > _new)
                {
                    return false;
                }
            }

            // All digits are equal therefore versions match.
            return false;
        }

        // IEvent implementation
        void IEvent.Execute() => CompareAndUpdate();

        // IDataFlow implementation
        SoftwareVersionsSoftwaresSoftware IDataFlow<SoftwareVersionsSoftwaresSoftware>.Data
        {
            set
            {
                newVersionValue = value;
                if (eventHasFired) CompareAndUpdate();
            }
        }
    }
}
