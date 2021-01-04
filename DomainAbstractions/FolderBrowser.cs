using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ProgrammingParadigms;

namespace DataLink_ALA.DomainAbstractions
{
    /// <summary>
    /// FolderBrowser is supposed to open a folder browser for select a folder object.
    /// Input ports:
    /// 1. IEvent: event to open the folder
    /// 2. ITableDataFlow: used to pass the folder path between ITableDataFlow abstractions
    /// 3. IDataFlow<string> used to set folder path implicitly
    ///
    /// Output ports:
    /// 1. IEvent: event output after user click OK button of the folder browser
    /// </summary>
    public class FolderBrowser : IEvent, ITableDataFlow, IDataFlow<string>
    {
        // properties
        public string InstanceName = "Default";

        // ports
        private IEvent folderSelected;
        private IEvent folderSelectError;
        private IDataFlow<string> selectedFolderPath;

        // fields
        private FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
        private string folderPath;
        private DataTable dataTable = new DataTable();
        private bool calledGetHeaderMethod = false;
        private int currentIndex = 0;
        private string browserTitle;

        // ctor
        public FolderBrowser(string title = "")
        {
            browserTitle = title;
        }

        // implementation of IDataFlow<string>
        string IDataFlow<string>.Data
        {
            set => folderPath = value;
        }

        // implementation of IEvent
        public void Execute()
        {
            folderBrowser.Description = browserTitle;

            DialogResult result = folderBrowser.ShowDialog();
            
            if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(folderBrowser.SelectedPath))
            {
                folderPath = folderBrowser.SelectedPath;
                if (selectedFolderPath != null)
                {
                    if (!Directory.Exists(folderPath))
                    {
                        folderSelectError?.Execute();
                    }
                    else
                    {
                        selectedFolderPath.Data = folderPath;
                        folderSelected?.Execute();
                    }

                }
            }

            folderBrowser.Reset();

        }
        // implementation of ITableDataFlow
        DataTable ITableDataFlow.DataTable => dataTable;
        async Task ITableDataFlow.GetHeadersFromSourceAsync(object queryOperation = null)
        {
            dataTable.Rows.Clear();
            dataTable.Columns.Clear();
            dataTable.Columns.Add("path");

            calledGetHeaderMethod = true;
            currentIndex = 0;
        }

        async Task<Tuple<int, int>> ITableDataFlow.GetPageFromSourceAsync()
        {
            if (string.IsNullOrEmpty(folderPath))
            {
                return new Tuple<int, int>(0, 0);
            }
            if (!calledGetHeaderMethod)
            {
                return new Tuple<int, int>(dataTable.Rows.Count, dataTable.Rows.Count);
            }

            DataRow r = dataTable.NewRow();
            r["path"] = folderPath;
            dataTable.Rows.Add(r);
            calledGetHeaderMethod = false;

            var tmpIndex = currentIndex;

            if (currentIndex < dataTable.Rows.Count)
            {
                currentIndex = dataTable.Rows.Count;
            }

            return new Tuple<int, int>(tmpIndex, dataTable.Rows.Count);
        }

        async Task ITableDataFlow.PutHeaderToDestinationAsync() { throw new NotImplementedException(); }

        async Task ITableDataFlow.PutPageToDestinationAsync(int firstRowIndex, int lastRowIndex, GetNextPageDelegate getNextPage) { throw new NotImplementedException();}

        DataRow ITableDataFlow.CurrentRow { get; set; }
        bool ITableDataFlow.SupportQuery { get; }
        bool ITableDataFlow.RequestQuerySupport() { return false; }
    }
}
