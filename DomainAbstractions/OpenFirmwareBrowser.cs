using Microsoft.Win32;
using ProgrammingParadigms;
using System.ComponentModel;
using System.Linq;

namespace DomainAbstractions
{
    /// <summary>
    /// Opens a file browser for firmware for an indicator or EID reader.
    /// Opened by an IEvent and triggers another IEvent when the file has
    /// been selected, which is pushed out on an IDataFlow<string> and an
    /// IDataFlowB<string>.
    /// -----------------------------------------------------------------
    /// Ports:
    /// 1. IEvent NEEDNAME: Opens the firmware browser.
    /// 2. IEvent fileSelected: Executed when the browser closes with a selected file.
    /// 3. IDataFlow<string> selectedFile: The file selected in the browser.
    /// 4. IDataFlowB<string> NEEDNAME: The file selected in the browser.
    /// </summary>
    class OpenFirmwareBrowser : IEvent, IDataFlowB<string>
    {
        public const string Scale5000Filter = "XR5000/ID5000 Firmware (*.tgz)|*.tgz";
        public const string EidReaderFilter = "XRS2/SRS2 Firmware (*.hex)|*.hex";

        // properties
        public string InstanceName = "Default";

        // outputs
        private IEvent fileSelected;
        private IDataFlow<string> selectedFile;

        // private properties
        private OpenFileDialog fileBrowser = new OpenFileDialog();
        private string filePath;

        /// <summary>
        /// Opens a file browser for firmware for an indicator or EID reader.
        /// Opened by an IEvent and triggers another IEvent when the file has
        /// been selected, which is pushed out on an IDataFlow<string> and an
        /// IDataFlowB<string>.
        /// </summary>
        /// <param name="title">Title of the window.</param>
        /// <param name="filter">Filter(s) to use in the selector.</param>
        public OpenFirmwareBrowser(string title, string filter)
        {
            fileBrowser.Title = title;
            fileBrowser.Multiselect = false;
            fileBrowser.Filter = filter;

            fileBrowser.FileOk += (object sender, CancelEventArgs e) =>
            {
                filePath = fileBrowser.FileNames.First();

                if (selectedFile != null) selectedFile.Data = filePath;
                DataChanged?.Invoke();
                fileSelected?.Execute();
            };
        }

        // IEvent implementation
        void IEvent.Execute()
        {
            fileBrowser.ShowDialog();
        }

        // IDataFlowB<string> implementation
        string IDataFlowB<string>.Data { get => filePath; }
        public event DataChangedDelegate DataChanged;
    }
}
