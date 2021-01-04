using ProgrammingParadigms;
using System.ComponentModel;
using System.Net;
using System.Threading.Tasks;

namespace DomainAbstractions
{
    /// <summary>
    /// Downloads a file to a given directory. Download is triggered by an IEvent and the URL can be set
    /// either via properties or an IDataFlow<string>. The progress is outputted through an IDataFlowB<string>.
    /// -------------------------------------------------------------------------------------------------------
    /// Ports:
    /// 1. IEvent NEEDNAME:                     Triggers the download. If there is no download URL, we wait until IDataFlow<string> NEEDNAME
    ///                                         provides one and then the download is started.
    /// 2. IDataFlow<string> NEEDNAME:          Provides the URL to download. Starts the download if IEvent NEEDNAME has already
    ///                                         been called.
    /// 3. IDataFlowB<int> NEEDNAME:            Provides progress updates as a percentage between 0 and 100, where 0 is not downloaded
    ///                                         and 100 is complete.
    /// 4. IDataFlowB<string> outputPathPort:   Sets the path to download the file to.
    /// 5. IDataFlow<int> downloadProgress:     Provides progress updates as a percentage between 0 and 100, where 0 is not downloaded
    ///                                         and 100 is complete.
    /// 5. IEvent downloadFinished:             Fired when the download has finished.
    /// </summary>
    class DownloadFile : IEvent, IDataFlow<string>, IDataFlowB<int>
    {
        // properties
        public string InstanceName = "Default";
        public string URL;
        public string outputPath;

        // ports
        private IDataFlowB<string> outputPathPort;
        private IDataFlow<int> downloadProgress;
        private IEvent downloadFinished;

        // private fields
        private WebClient client = new WebClient();
        private int progress = 0;
        private bool eventHasFired = false;

        /// <summary>
        /// Downloads a file to a given directory. Download is triggered by an IEvent and the URL can be set
        /// either via properties or an IDataFlow<string>. The progress is outputted through an IDataFlowB<string>.
        /// </summary>
        /// <param name="URL">The URL to download from.</param>
        /// <param name="outputPath">The path to store the file in.</param>
        public DownloadFile(string URL = default, string outputPath = default)
        {
            this.URL = URL;
            this.outputPath = outputPath;

            client.Headers.Add("User-Agent", Libraries.Constants.UserAgent);
            
            client.DownloadProgressChanged += new DownloadProgressChangedEventHandler((object sender, DownloadProgressChangedEventArgs e) =>
            {
                progress = e.ProgressPercentage;
                if (downloadProgress != null) downloadProgress.Data = progress;
                DataChanged?.Invoke();
            });

            client.DownloadFileCompleted += new AsyncCompletedEventHandler((object sender, AsyncCompletedEventArgs e) =>
            {
                downloadFinished?.Execute();
            });
        }

        /// <summary>
        /// Sets up the IDataFlowB output path port.
        /// </summary>
        private void PostWiringInitialize()
        {
            outputPathPort.DataChanged += () =>
            {
                outputPath = outputPathPort.Data;
                if (eventHasFired) StartDownload();
            };
        }

        /// <summary>
        /// Starts the file download.
        /// </summary>
        /// <returns>True if the download started, false otherwise.</returns>
        private bool StartDownload()
        {
            // wait until UTI and outputPath are set before downloading
            if (URL == default || outputPath == default) return false;

            client.DownloadFileAsync(new System.Uri(URL), outputPath);
            eventHasFired = false;
            return true;
        }

        // IEvent implementation
        void IEvent.Execute()
        {
            // if URL or outputPath are not present, set event has fired flag
            // so we can run when they are both present
            if (! StartDownload())
            {
                eventHasFired = true;
            }
        }

        // IDataFlow implementation
        string IDataFlow<string>.Data
        {
            set
            {
                URL = value;
                if (eventHasFired) StartDownload();
            }
        }

        // IDataFlowB implementation
        int IDataFlowB<int>.Data { get => progress; }

        public event DataChangedDelegate DataChanged;
    }
}
