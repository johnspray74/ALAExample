using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgrammingParadigms;
using System.ServiceModel;
using DataLink_ALA.NaitService;

namespace DomainAbstractions
{
    /// <summary>
    /// Connects a NaitService.ExternalClient to a proxy. This abstraction may hopefully later generalise to other types of clients if seamless casting between them is shown to be possible.
    /// ------------------------------------------------------------------------------------------------------------------
    /// Ports:
    /// 1. IDataFlowB<ExternalClient> ""NEEDNAME": 
    /// </summary>
    public class ConnectClientToProxy : IDataFlowB<ExternalClient>
    {
        // Properties ---------------------------------------------------------------
        public string InstanceName = "Default";
        public string ProxyUsername { set => proxyUsername = value; }
        public string ProxyPassword { set => proxyPassword = value; }
        public string IpAddress { set => ipAddress = value; }
        public int Port { set => port = value; }

        // Private fields ---------------------------------------------------------------
        private ExternalClient client;
        private string proxyUsername;
        private string proxyPassword;
        private string ipAddress;
        private int port;

        /// <summary>
        /// Connects a NaitService.ExternalClient to a proxy
        /// </summary>
        public ConnectClientToProxy() { }

        // IDataFlow<ExternalClient> implementation

        public event DataChangedDelegate DataChanged;
        ExternalClient IDataFlowB<ExternalClient>.Data
        {
            get
            {
                ConnectToProxy();
                return client;
            }
        }

        private void ConnectToProxy()
        {
            client = new ExternalClient();
            var binding = client.Endpoint.Binding as BasicHttpBinding;
            binding.ProxyAddress = new Uri($"http://{ipAddress}:{port}");
            binding.UseDefaultWebProxy = false;
            binding.Security.Mode = BasicHttpSecurityMode.Transport;
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
            binding.Security.Transport.ProxyCredentialType = HttpProxyCredentialType.Basic;
            binding.MaxReceivedMessageSize = Int32.MaxValue;
            if (client.ClientCredentials != null)
            {
                client.ClientCredentials.UserName.UserName = proxyUsername;
                client.ClientCredentials.UserName.Password = proxyPassword;
            }
        }
    }
}
