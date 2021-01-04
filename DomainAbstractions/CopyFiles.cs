using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using Newtonsoft.Json.Linq;
using ProgrammingParadigms;
using Libraries;

namespace DataLink_ALA.DomainAbstractions
{
 
    /// <summary>
    /// CopyFiles abstraction is supposed to copy files from source path to target path.
    /// Input ports:
    /// 1. ITableDataFlow: used to wired from abstractions like FolderBrowser to get the destination
    /// 2. IDataFlow<string>: used for source file input, if it is a folder, then copy all files in the folder to target;
    /// if it is a file, then only copy that file to target.
    ///
    /// Output ports:
    ///
    /// </summary>
    public class CopyFiles : ITableDataFlow, IDataFlow<string>
    {
        // properties
        public string InstanceName = "Default";

        // ports
        private IDataFlow<string> copyFilesCount;
        private IDataFlow<JObject> messageObj;

        // fields
        private string[] files;
        private DataTable dataTable = new DataTable();
        private string targetPath;

        // ctor


        // file source path, if is file, copy the file to target path; if is folder, copy all files in the folder to target
        string IDataFlow<string>.Data
        {
            set
            {
                var attr = File.GetAttributes(value);
                var isDirectory = attr.HasFlag(FileAttributes.Directory);

                if (messageObj != null) messageObj.Data = StaticUtilities.CreateJObjectMessage("INFORMATION", $"Received file source from {value}");

                files = isDirectory ? Directory.GetFiles(value) : new[] {value};

                if (messageObj != null) messageObj.Data = StaticUtilities.CreateJObjectMessage("INFORMATION", $"Total {files.Length.ToString()} will be copied.");

                if (copyFilesCount != null) copyFilesCount.Data = files.Length.ToString();
            }
        }

        DataTable ITableDataFlow.DataTable => dataTable;

        async Task ITableDataFlow.GetHeadersFromSourceAsync(object queryOperation)
        {
            throw new NotImplementedException();
        }

        async Task<Tuple<int, int>> ITableDataFlow.GetPageFromSourceAsync()
        {
            throw new NotImplementedException();
        }

        async Task ITableDataFlow.PutHeaderToDestinationAsync()
        {
        }

        async Task ITableDataFlow.PutPageToDestinationAsync(int firstRowIndex, int lastRowIndex, GetNextPageDelegate getNextPage)
        {
            if (dataTable.Rows.Count > 0)
            {
                targetPath = dataTable.Rows[0][0].ToString();
            }

            foreach (var s in files)
            {
                var fileName = Path.GetFileName(s);
                var destFile = Path.Combine(targetPath, fileName);
                File.Copy(s, destFile, true);
            }
        }

        DataRow ITableDataFlow.CurrentRow { get; set; }

        bool ITableDataFlow.SupportQuery { get; }

        bool ITableDataFlow.RequestQuerySupport() { return false; }
    }
}
