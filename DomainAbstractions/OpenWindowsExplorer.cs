using ProgrammingParadigms;
using System.Diagnostics;
using System.IO;

namespace DomainAbstractions
{
    /// <summary>
    /// It is basically a window which has the same UI style as a general window. All the files in the given
    /// file path will be listed in the window but only the given file name will be highlighted. 
    /// ------------------------------------------------------------------------------------------------------------------
    /// Ports
    /// 1. IDataFlow<string> "NEEDNAME": which inputs the file full path;
    /// 2. IEvent "NEEDNAME": which open the file window when it is triggered.
    /// </summary>
    public class OpenWindowsExplorer : IDataFlow<string>, IEvent
    {
        // properties
        public string InstanceName = "Default";

        // private fields
        private string filePath;

        /// <summary>
        /// A window explorer which aims to diaplay a specfic file in a file list with a given file full path.
        /// </summary>
        public OpenWindowsExplorer() { }

        // IDataFlow<string> implmentation -------------------------------------------------
        string IDataFlow<string>.Data { set => filePath = value; }


        // IEvent implementation -----------------------------------------------------------
        void IEvent.Execute()
        {
            var attr = File.GetAttributes(filePath);
            var isDirectory = attr.HasFlag(FileAttributes.Directory);

            if (isDirectory)
            {
                Process.Start(new ProcessStartInfo("explorer.exe", filePath));
            }
            else
            {
                Process.Start(new ProcessStartInfo("explorer.exe", " /select, " + filePath));
            }
        }
    }
}
