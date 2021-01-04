using Microsoft.Win32;
using ProgrammingParadigms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace DomainAbstractions
{
    /// <summary>
    /// A placeholder abstraction for when the wiring of a SaveFileBrowser needs to be used but the file browser itself should not be opened.
    /// </summary>
    public class AutoSave : IEvent
    {
        // properties ---------------------------------------------------------------
        public string InstanceName = "Default";

        // ports ---------------------------------------------------------------
        private IDataFlow<string> dataFlowOutputFilePath;
        private IDataFlow<int> dataFlowFileFormatIndex;
        private IEvent autoSaveHappened;

        /// <summary>
        /// A placeholder abstraction for when the wiring of a SaveFileBrowser needs to be used but the file browser itself should not be opened.
        /// </summary>
        public AutoSave() { }

        // IEvent implementation -------------------------------------------------------------------
        void IEvent.Execute()
        {
            string defaultPath = @"C:\ProgramData\Tru-Test\DataLink_ALA\" + "localSessions";
            if (!Directory.Exists(defaultPath)) Directory.CreateDirectory(defaultPath);
            dataFlowOutputFilePath.Data = defaultPath;
            dataFlowFileFormatIndex.Data = 3; // index 1 is for a plain csv file, index 3 is for the XR3000 format
            autoSaveHappened?.Execute();
        }
    }
}
