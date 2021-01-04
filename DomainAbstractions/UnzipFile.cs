using ProgrammingParadigms;
using System.IO;
using System.IO.Compression;
using System.Text.RegularExpressions;

namespace DomainAbstractions
{
    /// <summary>
    /// Takes a path to a ZIP file, searches through it and matches the name of the file with
    /// a regex pattern. When it matches, it unzips the file and outputs the path through an
    /// IDataFlow<string>. An IEvent is also fired when the file is unzipped. If no file can
    /// be unzipped, another IEvent is called.
    /// -------------------------------------------------------------------------------------
    /// Ports:
    /// 1. IEvent NEEDNAME: Starts the process.
    /// 2. IDataFlow<string> NEEDNAME: The path to the ZIP file.
    /// 3. IDataFlow<string> filePath: THe path to the extracted file.
    /// 4. IEvent fileUnzipped: Fired when the file is unzipped.
    /// 5. IEvent unzipFailed: Fired when no file can be unzipped.
    /// </summary>
    class UnzipFile : IEvent, IDataFlow<string>
    {
        // properties
        public string InstanceName = "Default";

        // ports
        private IDataFlow<string> filePath;
        private IEvent fileUnzipped;
        private IEvent unzipFailed;

        // private fields
        private string pattern;
        private string unzipLocation;
        private string zipLocation;
        private bool eventWasCalled = false;

        /// <summary>
        /// Takes a path to a ZIP file, searches through it and matches the name of the file with
        /// a regex pattern. When it matches, it unzips the file and outputs the path through an
        /// IDataFlow<string>. An IEvent is also fired when the file is unzipped. If no file can
        /// be unzipped, another IEvent is called.
        /// </summary>
        /// <param name="pattern">The pattern to match.</param>
        /// <param name="unzipLocation">The location to unzip to.</param>
        /// <param name="zipLocation">The location of the ZIP file.</param>
        public UnzipFile(string pattern, string unzipLocation, string zipLocation = default)
        {
            this.pattern = pattern;
            this.unzipLocation = unzipLocation;
            this.zipLocation = zipLocation;
        }

        /// <summary>
        /// Checks the ZIP file for a file that matches the pattern.  
        /// </summary>
        private void Unzip()
        {
            if (zipLocation == default)
            {
                eventWasCalled = true;
                return;
            }

            using (ZipArchive archive = ZipFile.OpenRead(zipLocation))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    if (Regex.IsMatch(entry.FullName, pattern))
                    {
                        string destPath = Path.GetFullPath(Path.Combine(unzipLocation, entry.Name));
                        entry.ExtractToFile(destPath, true);

                        if (filePath != null) filePath.Data = destPath;
                        fileUnzipped?.Execute();
                        
                        return;
                    }
                }
            }

            unzipFailed?.Execute();
        }

        // IEvent implementation
        void IEvent.Execute() => Unzip();

        // IDataFlow<string> implementation
        string IDataFlow<string>.Data
        {
            set
            {
                zipLocation = value;
                if (eventWasCalled) Unzip();
            }
        }
    }
}
