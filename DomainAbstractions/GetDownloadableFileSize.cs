using ProgrammingParadigms;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DomainAbstractions
{
    /// <summary>
    /// Takes a file URL and gets the size of the file in megabytes (by default). The number of
    /// decimal places and the metric prefix can be changed through properties.
    /// ------------------------------------------------------------
    /// Ports:
    /// 1. IEvent NEEDNAME: The event to trigger getting the file size.
    /// 2. IDataFlow<string>: The URL of the file. Will start getting the file size if IEvent has already been called.
    /// 3. IDataFlow<double> fileSize: The size of the file.
    /// 4. IEvent taskComplete: Fired when the file size has been retrieved.
    /// </summary>
    class GetDownloadableFileSize : IEvent, IDataFlow<string>
    {
        // properties
        public string InstanceName = "Default";
        public string URL;
        public int decimalPlaces = 2;
        public int exponent = 6;

        // ports
        private IDataFlow<double> fileSize;
        private IEvent taskComplete;

        // private 
        private bool eventHasExecuted = false;
        private HttpClient client = new HttpClient();

        /// <summary>
        /// Takes a file URL and gets the size of the file in megabytes (by default). The number of
        /// decimal places and the metric prefix can be changed through properties.
        /// </summary>
        /// <param name="URL">The file URL to get the size of.</param>
        public GetDownloadableFileSize(string URL = default)
        {
            this.URL = URL;
            client.DefaultRequestHeaders.UserAgent.ParseAdd($"User-Agent {Libraries.Constants.UserAgent}");
        }

        /// <summary>
        /// Gets the file size of the given file.
        /// </summary>
        /// <returns>Task to await.</returns>
        async Task GetFileSize()
        {
            // reset lock
            eventHasExecuted = false;
            
            // build request and wait for response
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Head, URL);
            HttpResponseMessage response = await client.SendAsync(request);

            // convert content length to megabytes
            double contentLength = (double)response.Content.Headers.ContentLength;
            contentLength /= Math.Pow(10, exponent);

            if (fileSize != null)
            {
                // round content length to given number of DP
                fileSize.Data = Math.Round(contentLength, decimalPlaces);
            }

            taskComplete?.Execute();
        }

        // IEvent implementation
        void IEvent.Execute()
        {
            // when IEvent is fired, if the URL has not been set
            // we will wait until it is set via IDataFlow<string> and THEN get the size
            if (URL == default)
            {
                eventHasExecuted = true;
            }
            else
            {
                Task _ = GetFileSize();
            }
        }

        // IDataFlow implementation
        string IDataFlow<string>.Data
        {
            set
            {
                URL = value;

                // if IEvent has already been fired and we have just got the URL,
                // get the file size.
                if (eventHasExecuted)
                {
                    Task _ = GetFileSize();
                }
            }
        }
    }
}
