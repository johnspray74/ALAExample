using System;
using System.Windows;
using System.Windows.Media;
using System.IO;
using ProgrammingParadigms;
using Libraries;

namespace DomainAbstractions
{
    /// <summary>
    /// MainWindow is an ALA domain abstraction
    /// Abstraction description follows:
    /// This is the main window of an application.
    /// The IUI port should be wired to children UI domain abstractions that are to be displayed inside the main window. 
    /// The output IEvent port appStart fires once the MainWindow is running and displayed
    /// ------------------------------------------------------------------------------------------------------------------
    /// Configurations: (configurations are for use by the application when it instantiates a domain abstraction)
    ///     title (parameter of the constructor): is for the application to configure the title for the title bar of the main window
    ///     InstanceName property: As with all domain abstractions, we have an instance name. (Because there can be multiple instances of this abstraction, the application gives us an object name which is not generally used by the abstraction internal logic. It is only used during debugging so you can tell which object you are break-pointed on.
    ///     The public method "Run" is unusual for a domain abstraction. It should be called by the application after all wiring and initialization is complete to start the application actually running.
    /// ------------------------------------------------------------------------------------------------------------------
    /// Ports:
    ///     close (IEvent): input to close the window and exit the application
    ///     enable (IDataFlow<bool>): input false disables (greys out) the UI
    ///     children (IUI): all the IUI children of the MainWindow
    ///     appStart (IEvent): outputs an event once the window is loaded
    ///     appClosing (IEvent): outputs and event when the window is closing
    /// <summary>

    public class MainWindow : IEvent, IDataFlow<bool> // close, enable
    {
        // properties -----------------------------------------------------------------
        public string InstanceName { get; set; } = "Default";




        // ports -----------------------------------------------------------------
        private IUI iuiStructure;
        private IEvent appStart;
        private IEvent appClosing;
        private IEventB restoreWindow;




        // private fields -----------------------------------------------------------------
        private Window window;




        /// <summary>
        /// Constructor instantiates a main UI window for the application.
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
                appStart?.Execute();
            };

            window.Closing += (s, e) =>
            {
                if (!string.IsNullOrEmpty(logArchiveFilePath)) File.Copy(Logging.logFilePath, logArchiveFilePath);
                appClosing?.Execute();
            };

            window.Closed += (object sender, EventArgs e) => ((IEvent)this).Execute();
        }




        // This method is the main entry point to start the application. Call after all wiring and initilization is completed.
        public void Run()
        {
            window.Content = iuiStructure.GetWPFElement();
            System.Windows.Application app = new System.Windows.Application();
            Console.WriteLine("MainWindow Running");
            app.Run(window);
        }




        // By having this name convention, this method gets called by WireTo immediately after the correspeonding port is wired
        private void restoreWindowInitialize()
        {
            restoreWindow.EventHappened += () =>
            {
                if (window.WindowState == WindowState.Minimized) window.WindowState = WindowState.Normal;
                else if (window.WindowState == WindowState.Normal) window.WindowState = WindowState.Minimized;
            };
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
