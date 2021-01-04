using ProgrammingParadigms;
using System;
using System.Reflection;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DomainAbstractions
{
    /// <summary>
    /// A menu item of a menu that can be clicked. Has a IEvent port on the RHS. 
    /// When the item is clicked, it generates an event.
    /// Ports
    /// 1. IUI 
    /// 2. IDataFlow<bool> Visible
    /// </summary>
    public class MenuItem : IUI, IDataFlow<bool>
    {
        // properties
        public string InstanceName = "Default";
        public string IconName
        {
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    menuItem.Icon = new System.Windows.Controls.Image()
                    {
                        Source = new BitmapImage(new Uri(@"pack://application:,,,/" + Assembly.GetExecutingAssembly().GetName().Name + @";component/Application/" + string.Format("Resources/{0}", value), UriKind.Absolute)),
                        Height = 20,
                        MaxWidth = 25
                    };
                }
            }
        }

        // outputs
        private IEvent eventOutput;
        private IDataFlowB<bool> dataFlowBOutput;

        // private fields
        private System.Windows.Controls.MenuItem menuItem;

        /// <summary>
        /// An IUI element which is a windows-desktop application style MenuItem. 
        /// </summary>
        /// <param name="title">the text displayed on the menu item</param>
        public MenuItem(string title, bool visible = true)
        {
            menuItem = new System.Windows.Controls.MenuItem();
            menuItem.Header = title;
            menuItem.VerticalAlignment = VerticalAlignment.Center;
            menuItem.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            menuItem.FontSize = 14;
            menuItem.Click += (object sender, RoutedEventArgs e) => eventOutput?.Execute();
            menuItem.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
        }

        private void PostWiringInitialize()
        {
            if (dataFlowBOutput != null)
            {
                dataFlowBOutput.DataChanged += () =>
                {
                    menuItem.IsEnabled = dataFlowBOutput.Data;
                    menuItem.Foreground = menuItem.IsEnabled ? new SolidColorBrush(Color.FromRgb(0, 0, 0)) : Brushes.DarkGray;
                };
            }
        }

        // IUI implmentation -----------------------------------------------------------
        UIElement IUI.GetWPFElement() => menuItem;

        // IDataFlow<bool> implementation -----------------------------------------------
        bool IDataFlow<bool>.Data { set => menuItem.Visibility = value ? Visibility.Visible : Visibility.Collapsed; }
    }
}
