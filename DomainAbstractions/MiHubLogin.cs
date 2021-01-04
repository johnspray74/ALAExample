using ProgrammingParadigms;

namespace DomainAbstractions
{
    public class MiHubLogin : IEvent, IDataFlow<bool>
    {
        /// <summary>
        /// [DEPRECATED]
        /// </summary>

        // Properties
        public string InstanceName = "Default";

        // Fields
        private bool credentialsAreValid;

        // Outputs
        private IDataFlow<bool> requireLogin;
        private IEvent startUpload;


        public MiHubLogin() { }

        // IEvent implementation
        void IEvent.Execute()
        {
            if (credentialsAreValid)
            {
                requireLogin.Data = false;
                startUpload.Execute();
            }
            else
            {
                requireLogin.Data = true;
            }
        }

        // IDataFlow<bool> implementation
        bool IDataFlow<bool>.Data { set => credentialsAreValid = value; }
    }
}
