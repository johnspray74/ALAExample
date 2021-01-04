using ProgrammingParadigms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DomainAbstractions
{
    /// <summary>
    /// A simple abstraction which outputs file name and file create date with the
    /// given directory path. The output would be pulled by other object one by one
    /// through IIterator interface.
    /// ------------------------------------------------------------------------------------------------------------------
    /// Ports:
    /// 1. IIterator<Tuple<string, string>> "NEEDNAME": data pulled by other object one by one through IIterator interface
    /// 2. IDataFlow<string> "NEEDNAME": for inputting directory path
    /// </summary>
    public class ListOfFiles : IIterator<Tuple<string, string>>, IDataFlow<string>
    {
        // properties -----------------------------------------------------------------
        public string InstanceName = "Default";

        // private fields -----------------------------------------------------------------
        private int index = 0;
        private int[] orderedFileIndex;
        private Dictionary<string, string> filesDictionary = new Dictionary<string, string>();

        /// <summary>
        /// Generate file list which includes file name and file create date through
        /// IIterator interface.
        /// </summary>
        public ListOfFiles() { }

        // IDataFlow<string> implementation --------------------------------------------
        string IDataFlow<string>.Data
        {
            set
            {
                index = 0;

                if (!Directory.Exists(value)) return;

                string[] files = Directory.GetFiles(value);

                filesDictionary.Clear();

                foreach (string f in files)
                {
                    var fileName = Path.GetFileName(f);
                    var sessionIndex = fileName.Substring(0, fileName.IndexOf("_"));

                    if (!filesDictionary.ContainsKey(sessionIndex))
                    {
                        filesDictionary.Add(sessionIndex, f);
                    }
                }

                orderedFileIndex = filesDictionary.Select(kvp => Convert.ToInt32(kvp.Key)).ToArray();
                Array.Sort(orderedFileIndex);
            }
        }

        // IIterator<Tuple<string, string>> implementation -----------------------------
        bool IIterator<Tuple<string, string>>.Finished
        {
            get
            {
                if (index >= orderedFileIndex.Length)
                    return true;
                return false;
            }
        }

        async Task<Tuple<string, string>> IIterator<Tuple<string, string>>.Next()
        {
            string fileName = filesDictionary[orderedFileIndex[index++].ToString()];
            string fileDate = File.GetLastWriteTime(fileName).ToString("dd/MM/yyyy");

            return new Tuple<string, string>(fileName, fileDate);
        }

        void IIterator<Tuple<string, string>>.Reset()
        {
            index = 0;
        }

    }
}
