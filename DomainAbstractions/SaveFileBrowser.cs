using Microsoft.Win32;
using ProgrammingParadigms;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.IO;

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
    /// </summary>
    public class SaveFileBrowser : IEvent, IDataFlow<string> // showWindow, initialSaveFileName
    {
        // properties
        public string InstanceName = "Default";

        // outputs
        private List<IDataFlow<string>> dataFlowOutputFileNames = new List<IDataFlow<string>>();
        private List<IDataFlow<string>> dataFlowOutputFilePaths = new List<IDataFlow<string>>();
        private List<IDataFlow<string>> dataFlowOutputFilePathNames = new List<IDataFlow<string>>();
        private IDataFlow<int> dataFlowFileFormatIndex;

        // private fields
        private SaveFileDialog saveFileDialog = new SaveFileDialog();
        private Dictionary<string, string> filterTypes = new Dictionary<string, string>()
        {
            { "CSV", "Datamars CSV file(*.csv)|*.csv|" },
            { "CSV No Header", "Datamars CSV file No Header(*.csv)|*.csv|" },
            { "CSV 3000 Format", "Datamars CSV file 3000 format(*.csv)|*.csv|" },
            { "CSV Minda Format", "Datamars CSV file Minda format(*.csv)|*.csv|" },
            { "CSV EID only", "Datamars CSV file EID only(*.csv)|*.csv|" },
            { "Excel 97-2003", "Microsoft Excel 97-2003 Worksheet(*.xls)|*.xls|" },
            { "Excel Worksheet", "Microsoft Excel Worksheet(*.xlsx)|*.xlsx|" },
            { "Excel 97-2003 from Template", "Microsoft Excel 97-2003 Worksheet from Template File(*.xls)|*.xls|" },
            { "Datamars XML", "Datamars XML file(*.xml)|*.xml|" },
            { "Tru-Test Favourite", "Tru- Test favourite file(*.ttfav)|*.ttfav|" },
            { "All files", "All files(*.*)|*.*" }
        };

        /// <summary>
        /// Opening a file browser and save a file or mutiple files to the folder path opened.
        /// Please refer to filter keys within class for specific file filter.
        /// </summary>
        /// <param name="title">the text diaplayed on the window top border</param>
        public SaveFileBrowser(string title = null, string filter = "Default")
        {
            saveFileDialog.Title = title;

            string filterString = customiseFilter(filter);

            saveFileDialog.Filter = filterString;

            saveFileDialog.FileOk += (object sender, CancelEventArgs e) => {
                if (dataFlowFileFormatIndex != null) dataFlowFileFormatIndex.Data = saveFileDialog.FilterIndex;

                foreach (var i in dataFlowOutputFileNames) i.Data = Path.GetFileName(saveFileDialog.FileName);
                foreach (var i in dataFlowOutputFilePaths) i.Data = Path.GetDirectoryName(saveFileDialog.FileName);
                foreach (var i in dataFlowOutputFilePathNames) i.Data = saveFileDialog.FileName;
            };
        }

        // IDataFlow<string> implementation --------------------------------------------------------
        string IDataFlow<string>.Data { set => saveFileDialog.FileName = value; }

        // IEvent implementation -------------------------------------------------------------------
        void IEvent.Execute()
        {
            saveFileDialog.ShowDialog();
        }

        //private method ---------------------------------------------------------
        private string customiseFilter(string filter)
        {
            string customisedFilter = null;

            switch (filter)
            {
                case "CSV": 
                    customisedFilter += filterTypes["CSV"];
                    break;
                case "CSV No Header":
                    customisedFilter += filterTypes["CSV No Header"];
                    break;
                case "CSV 3000 Format":
                    customisedFilter += filterTypes["CSV 3000 Format"];
                    break;
                case "CSV Minda Format":
                    customisedFilter += filterTypes["CSV Minda Format"];
                    break;
                case "CSV EID only":
                    customisedFilter += filterTypes["CSV EID only"];
                    break;
                case "Excel 97-2003":
                    customisedFilter += filterTypes["Excel 97-2003"];
                    break;
                case "Excel Worksheet":
                    customisedFilter += filterTypes["Excel Worksheet"];
                    break;
                case "Excel 97-2003 from Template":
                    customisedFilter += filterTypes["Excel 97-2003 from Template"];
                    break;
                case "Datamars XML":
                    customisedFilter += filterTypes["Datamars XML"];
                    break;
                case "Tru-Test Favourite":
                    customisedFilter += filterTypes["Tru-Test Favourite"] += filterTypes["All files"];
                    break;
                case "All files":
                    customisedFilter += filterTypes["All files"];
                    break;
                case "Default":
                    foreach(KeyValuePair<string,string> entry in filterTypes)
                    {
                        customisedFilter += entry.Value;
                    }
                    break;
            }

            return customisedFilter;
        }
    }
}
