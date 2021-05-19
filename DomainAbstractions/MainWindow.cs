using ProgrammingParadigms;
using System;
using System.Reflection;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using Libraries;
using System.Windows.Forms;

namespace DomainAbstractions
{
    /// <summary>
    /// This is the main window of the application. The output IUI in it takes the responsibility of getting the WPF UIElements 
    /// in a hierarchical calling order as it goes deeper in the abstrations wired to it. The output IEvent starts to execute once 
    /// the app starts running, which informs the abstraction who implements it to do the things it wants.
    /// ------------------------------------------------------------------------------------------------------------------
    /// Ports:
    /// 1. IEvent shutdown: input for close the window (exits the application)
    /// 2. IDataFlow<bool> enable: to enable(true) or disable(false, grey out) the UI
    /// 3. IUI iuiStructure: all the IUI contained within the MainWindow
    /// 4. IEvent appStartsRun: IEvent that is pushed out once window has been loaded
    /// <summary>

    public class MainWindow : IEvent, IDataFlow<bool> // shutdown, enable
    {
        // properties -----------------------------------------------------------------
        public string InstanceName = "Default";

        // ports -----------------------------------------------------------------
        private IUI iuiStructure;
        private IEvent appStartsRun;
        private IEvent appClosing;
        private IEventB restoreWindow;

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
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };

            window.StateChanged += new EventHandler(HandleStateChanged);

            window.Loaded += (object sender, RoutedEventArgs e) =>
            {
                this.HandleStateChanged(sender, e);
                appStartsRun?.Execute();
            };

            window.Closing += (s, e) =>
            {
                if (!string.IsNullOrEmpty(logArchiveFilePath)) File.Copy(Logging.logFilePath, logArchiveFilePath);
                appClosing?.Execute();
            };

            window.Closed += (object sender, EventArgs e) => ((IEvent)this).Execute();
        }

        // private method -------------------------------------------------------
        private void PostWiringInitialize()
        {
            if (restoreWindow != null) restoreWindow.EventHappened += () =>
            {
                if (window.WindowState == WindowState.Minimized) window.WindowState = WindowState.Normal;
                else if (window.WindowState == WindowState.Normal) window.WindowState = WindowState.Minimized;
            };
        }

        public void Run()
        {
            window.Content = iuiStructure.GetWPFElement();
            System.Windows.Application app = new System.Windows.Application();
            app.Run(window);
        }

        private void HandleStateChanged(object sender, EventArgs e)
        {
            window.ShowInTaskbar = !(window.WindowState == WindowState.Minimized);
        }

        // IEvent implementation -------------------------------------------------------
        void IEvent.Execute() => System.Windows.Application.Current.Shutdown();

        // IDataFlow<bool> implementation ----------------------------------------------
        bool IDataFlow<bool>.Data { set => window.IsEnabled = value; }
    }
}
