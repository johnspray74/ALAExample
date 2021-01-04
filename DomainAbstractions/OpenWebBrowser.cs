using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Text.RegularExpressions;
using ProgrammingParadigms;

namespace DomainAbstractions
{
    /// <summary>
    /// Abstraction class for opening a web browser url and has a method to test for validity of the URL.
    /// ------------------------------------------------------------------------------------------------------------------
    /// Ports:
    /// IUI "start": input IEvent to initiate the opening of the URL
    /// </summary>
    public class OpenWebBrowser : IEvent
    {
        // properties
        public string InstanceName = "Default";
        private string URL;

        // outputs
        private IEvent websiteOpened;

        /// <summary>
        /// Abstraction class for opening a web browser url
        /// </summary>
        /// <param name="URL"></param>
        public OpenWebBrowser(string URL = "") 
        {
            this.URL = URL;
        }

        // Leave public for unit tests
        public bool IsValidUrl(string URL)
        {
            var regex = new Regex(@"^(?:http(s)?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+$");
            return regex.IsMatch(URL);
        }

        // IEvent implementation
        void IEvent.Execute()
        {
            if (IsValidUrl(URL))
            {
                Process.Start(URL);
                websiteOpened?.Execute();
            }
        }
    }
}
