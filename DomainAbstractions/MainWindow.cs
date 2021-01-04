using ProgrammingParadigms;
using System;
using System.Reflection;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using Libraries;

namespace DomainAbstractions
{
    /// <summary>
    /// This is the main window of the application. The output IUI in it takes the responsibility of getting the WPF UIElements 
    /// in a hierarchical calling order as it goes deeper in the abstrations wired to it. The output IEvent starts to execute once 
    /// the app starts running, which informs the abstraction who implements it to do the things it wants.
    /// ------------------------------------------------------------------------------------------------------------------
    /// Ports:
    /// 1. IEvent Shutdown: input for close the window (exits the application)
    /// 2. IDataFlow<bool> "NEEDNAME": to enable(true) or disable(false, grey out) the UI
    /// 3. IUI iuiStructure: all the IUI contained within the MainWindow
    /// 4. IEvent appStartsRun: IEvent that is pushed out once window has been loaded
    /// <summary>

    public class MainWindow : IEvent, IDataFlow<bool>
    {
        // properties -----------------------------------------------------------------
        public string InstanceName = "Default";

        // ports -----------------------------------------------------------------
        private IUI iuiStructure;
        private IEvent appStartsRun;

        // private fields -----------------------------------------------------------------
        private Window window;

        /// <summary>
        /// Generates the main UI window of the application and emits a signal that the Application starts running.
        /// </summary>
        /// <param name="title">title of the window</param>
        public MainWindow(string title = null, string logArchiveFilePath = "")
        {
            window = new Window()
            {
                Title = title,
                Height = SystemParameters.PrimaryScreenHeight * 0.65,
                Width = SystemParameters.PrimaryScreenWidth * 0.6,
                MinHeight = 500,
                MinWidth = 750,
                Background = Brushes.White,
                Icon = new BitmapImage(new Uri(@"pack://application:,,,/" + Assembly.GetExecutingAssembly().GetName().Name + @";component/Application/Resources/DataLink.ico", UriKind.Absolute)),
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };

            window.Loaded += (object sender, RoutedEventArgs e) =>
            {
                appStartsRun?.Execute();
            };
            window.Closing += (s, e) =>
            {
                if (!string.IsNullOrEmpty(logArchiveFilePath)) File.Copy(Logging.logFilePath, logArchiveFilePath);
            };
            window.Closed += (object sender, EventArgs e) => ((IEvent)this).Execute();

        }

        // private method -------------------------------------------------------
        public void Run()
        {
            window.Content = iuiStructure.GetWPFElement();
            System.Windows.Application app = new System.Windows.Application();
            app.Run(window);
        }


        // IEvent implementation -------------------------------------------------------
        void IEvent.Execute() => System.Windows.Application.Current.Shutdown();

        // IDataFlow<bool> implementation ----------------------------------------------
        bool IDataFlow<bool>.Data { set => window.IsEnabled = value; }
    }
}
