using ProgrammingParadigms;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace DomainAbstractions
{
    /// <summary>
    /// Calculates and compares the MD5 hash of a file with a given hash.
    /// Will not check if the file exists.
    /// -----------------------------------------------------------------
    /// Ports:
    /// 1. IEvent NEEDNAME:                     Starts the calculation and compare.
    /// 2. IDataFlowB<string> compareHashPort:  The known hash to compare to.
    /// 3. IDataFlowB<string> filePathPort:     The path to the file to calculate the hash for.
    /// 4. IDataFlow<string> match:             Whether the two hashes match.
    /// 5. IEvent hashMatch:                    Called when the hashes match.
    /// 6. IEvent hashNoMatch:                  Called when the hashes do not match.
    /// </summary>
    class CheckFileMD5Hash : IEvent
    {
        // properties
        public string InstanceName = "Default";

        // ports
        private IDataFlowB<string> compareHashPort;
        private IDataFlowB<string> filePathPort;
        private IDataFlow<bool> match;
        private IEvent hashMatch;
        private IEvent hashNoMatch;

        // private fields
        private string compareHash;
        private string filePath;

        /// <summary>
        /// Calculates and compares the MD5 hash of a file with a given hash.
        /// Will not check if the file exists.
        /// </summary>
        /// <param name="compareHash">The hash to compare to.</param>
        /// <param name="filePath">The file to calculate the hash for.</param>
        public CheckFileMD5Hash(string compareHash = default, string filePath = default)
        {
            this.compareHash = default;
            this.filePath = default;
        }

        /// <summary>
        /// Sets up the listeners for the compare hash and file path ports.
        /// </summary>
        private void PostWiringInitialize()
        {
            if (compareHashPort != null)
            {
                compareHashPort.DataChanged += () => { compareHash = compareHashPort.Data; };
            }

            if (filePathPort != null)
            {
                filePathPort.DataChanged += () => { filePath = filePathPort.Data; };
            }
        }

        /// <summary>
        /// Compares the two hashes.
        /// </summary>
        /// <returns>Whether the two hashes match.</returns>
        private bool CompareHashes()
        {
            return CalculateFileHash() == compareHash;
        }

        /// <summary>
        /// Calculates the hash of the file.
        /// </summary>
        /// <returns>The hash of the file.</returns>
        private string CalculateFileHash()
        {
            using (MD5 md5 = MD5.Create())
            {
                using (FileStream stream = File.OpenRead(filePath))
                {
                    byte[] hashByte = md5.ComputeHash(stream);
                    return hashByte.Select(b => string.Format("{0:x}", b)).Aggregate((a, b) => a + b);
                }
            }
        }

        // IEvent implementation
        void IEvent.Execute()
        {
            // can't compare without something to compare to
            if (compareHash == default || filePath == default) return;
            bool result = CompareHashes();

            if (result)
            {
                hashMatch?.Execute();
            }
            else
            {
                hashNoMatch?.Execute();
            }

            if (match != null)
            {
                match.Data = result;
            }
        }
    }
}
