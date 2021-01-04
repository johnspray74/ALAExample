using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security;
using ProgrammingParadigms;

namespace   DomainAbstractions
{
    public class PasswordEncryption : IDataFlow<string>
    {
        /// <summary>
        /// [DEPRECATED]
        /// This abstraction acts as a black box to allow encrypting passwords with whichever methods deemed appropriate. Converts a regular string to a SecureString.
        /// </summary>

        // Properties
        public string InstanceName = "Default";

        // Outputs
        IDataFlow<string> encryptedPasswordOutput;
        private string encryptedPassword;


        public PasswordEncryption()
        {
            encryptedPassword = encryptPassword(encryptedPassword);
        }

        private string encryptPassword(string password)
        {
            return password; // No encryption for now
        }

        // IDataFlow<string> implementation
        string IDataFlow<string>.Data { set => encryptedPassword = value; }

    }
}
