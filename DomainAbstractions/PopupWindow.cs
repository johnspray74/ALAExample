using ProgrammingParadigms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DomainAbstractions
{
    /// <summary>
    /// A GUI element that is a stand alone window.
    /// This is a general window which can be used as any popup in the application. Initially the window is not visible.
    /// It can be customized on the Width, Height, Title, Resize (SizeToContent), and show button to close or not by sending parameters in constructor.
    /// Clicked events are sent to the contained widget.
    /// --------------------------------------------------------------------------------------------
    /// Ports:
    /// 1. IEvent openOrCloseWindow: for closing the window (if it's active) or open the window (if it's not active)
    /// 2. IDataFlow<bool> visible: to enable (true) or disable (false, grey out) the UI
    /// 3. IDataFlow<string> title: to set the popup window title
    /// 4. List<IUI> children: output list of IU elements contained in this PopUpWindow in order of wiring
    /// </summary>
    public class PopupWindow : IEvent, IDataFlow<bool>, IDataFlow<string> // openOrCloseWindow, visible, title
    {
        // properties ---------------------------------------------------------------------
        public string InstanceName = "Default";
        public double Width;
        public double Height;

        public SizeToContent Resize
        {
            set
            {
                window.SizeToContent = value;
            }
        }

        // ports ---------------------------------------------------------------------
        private List<IUI> children = new List<IUI>();

        // private fields ---------------------------------------------------------------------
        private Window window = new Window();
        private StackPanel backgroundPanel = new StackPanel() { Background = new SolidColorBrush(Color.FromRgb(235, 235, 235)) };

        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hwnd, int nIndex);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hwnd, int nIndex, int dwNewLong);

        /// <summary>
        /// A general parent window which includes customized WPF elements.
        /// </summary>
        /// <param name="title">the text displayed on the top border of the window</param>
        /// <param name="showCloseMiniMaxButton">control the displaying of close, minimum and maximum button</param>
        public PopupWindow(string title = "", bool showCloseMiniMaxButton = true)
        {
            window.Title = title;
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.Topmost = true;
            window.Content = backgroundPanel;

            if (!showCloseMiniMaxButton)
            {
                // hide close, minimun and maximum button
                window.Loaded += (object sender, RoutedEventArgs e) => {
                    var hwnd = new System.Windows.Interop.WindowInteropHelper(window).Handle;
                    var windowLong = GetWindowLong(hwnd, -16) & ~0x80000;
                    //var windowLong = GetWindowLong(hwnd, GWL_STYLE) & -131073 & -65537;
                    SetWindowLong(hwnd, -16, windowLong);
                };
            }
            else
            {
                // This event is added for handing the close action by user, as a window instance cannot be open again after it is 
                // closed by clicking on the close button. However, we can reopen it after it's hided so here we use hide() to make it look like closed.
                window.Closing += (object sender, CancelEventArgs e) => {
                    e.Cancel = true;
                    window.Hide();
                };
            }

            window.SizeToContent = SizeToContent.Height; // Resizes the popup window to match the height of its contained contents
        }


        private void PostWiringInitialize()
        {
            foreach (var c in children)
            {
                backgroundPanel.Children.Add(c.GetWPFElement());
            }
        }

        // IEvent implementation --------------------------------------
        void IEvent.Execute() => ShowWindow(!window.IsVisible);

        // IDataFlow<bool> implementation -----------------------------
        bool IDataFlow<bool>.Data { set => ShowWindow(value); }

        string IDataFlow<string>.Data { set => window.Title = value; }

        // private methods --------------------------------------------
        private void ShowWindow(bool show)
        {
            if (show)
            {
                window.Width = Width;
                window.Height = Height;
                window.Show();
            }
            else
            {
                window.Hide();
            }
        }
    }
}
