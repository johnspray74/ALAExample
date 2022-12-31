using Microsoft.Win32;
using ProgrammingParadigms;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.IO;
using Foundation;
using System;

namespace DomainAbstractions
{
    /// <summary>
    /// The aim of this abstraction is to get a file save path by user selecting the folder manually.
    /// It has an input box for inputting the file name the user wants to save.
    /// It has a save button, when clicked on that, it generates the file name, path and full path for outputting.
    /// It also outputs the file format index
    /// ------------------------------------------------------------------------------------------------------------------
    /// Ports:
    /// 1. IEvent showWindow: for opening the browser.
    /// 2. IDataFlow<string> initialSaveFileName: for initialization of file name of the browser.
    /// 3. List<IDataFlow<string>> dataFlowOutputFileNames: for output file name
    /// 4. List<IDataFlow<string>> dataFlowOutputFilePaths: for output file paths
    /// 5. List<IDataFlow<string>> dataFlowOutputFilePathNames: for output file path names
    /// 6. IDataFlow<int> dataFlowFileFormatIndex: selected index for the filter
    /// 7. IEvent fileSelected: Fired when a file has been selected.
    /// </summary>
    public class SaveFileBrowser : IEvent, IDataFlow<string> // showWindow, initialSaveFileName
    {
        // properties
        public string InstanceName = "Default";

        public Dictionary<string, string> Filters { set => saveFileDialog.Filter = String.Join("|", value.Values); }
        public string FileName { set => saveFileDialog.FileName = value; }

        // ports
        private List<IDataFlow<string>> dataFlowOutputFileNames = new List<IDataFlow<string>>();
        private List<IDataFlow<string>> dataFlowOutputFilePaths = new List<IDataFlow<string>>();
        private List<IDataFlow<string>> dataFlowOutputFilePathNames = new List<IDataFlow<string>>();
        private IDataFlow<int> dataFlowFileFormatIndex;
        private IEvent fileSelected;

        // private fields
        private SaveFileDialog saveFileDialog = new SaveFileDialog();

        /// <summary>
        /// Opening a file browser and save a file or mutiple files to the folder path opened.
        /// Please refer to filter keys within class for specific file filter.
        /// </summary>
        /// <param name="title">the text diaplayed on the window top border</param>
        public SaveFileBrowser(string title = null, string extension = "*")
        {
            saveFileDialog.Title = title;

            string filterString = $"{extension} file (*.{extension})|*.{extension}";

            saveFileDialog.Filter = filterString;

            saveFileDialog.FileOk += (object sender, CancelEventArgs e) => {
                if (dataFlowFileFormatIndex != null) dataFlowFileFormatIndex.Push(saveFileDialog.FilterIndex);

                foreach (var i in dataFlowOutputFileNames) i.Push(Path.GetFileName(saveFileDialog.FileName));
                foreach (var i in dataFlowOutputFilePaths) i.Push(Path.GetDirectoryName(saveFileDialog.FileName));
                foreach (var i in dataFlowOutputFilePathNames) i.Push(saveFileDialog.FileName);

                fileSelected?.Execute();
            };
        }

        // IDataFlow<string> implementation --------------------------------------------------------
        void IDataFlow<string>.Push(string data) { saveFileDialog.FileName = data; }

        // IEvent implementation -------------------------------------------------------------------
        void IEvent.Execute()
        {
            saveFileDialog.ShowDialog();
        }

    }
}
