using ProgrammingParadigms;
using System.Windows;
using Libraries;

namespace DomainAbstractions
{
    /// <summary>
    /// This abstraction is a general button which can be customized by it's Title, Width, Height, FontSize and Margin. 
    /// It is an interactive UI element which generates an IEvent when clicked.
    /// ------------------------------------------------------------------------------------------------------------------
    /// Ports:
    /// IUI "NEEDNAME": input IUI to get the WPF element
    /// IDataFlow "NEEDNAME": sets whether the button is enabled or not
    /// IEvent "eventButtonClicked": output event when button is clicked
    /// </summary>
    public class Button : IUI, IDataFlow<bool>
    {
        // properties ------------------------------------------------------------
        public string InstanceName = "Default";
        // the properties can extend for any UI customizing requirements
        public double Width { set => button.Width = value; }
        public double Height { set => button.Height = value; }
        public double FontSize { set => button.FontSize = value; }
        public Thickness Margin { set => button.Margin = value; }

        // ports ---------------------------------------------------------------
        private IEvent eventButtonClicked;

        // private fields --------------------------------------------------------
        private System.Windows.Controls.Button button;

        /// <summary>
        /// An interactive UI element which omits an IEvent output when clicked.
        /// It can be customized by setting the Title, Width, Height FontSize and Margin.
        /// </summary>
        /// <param name="title">The text displayed on the button</param>
        /// <param name="enabled">Whether the button is initially enabled</param>
        public Button(string title, bool enabled = true)
        {
            button = new System.Windows.Controls.Button();
            button.Content = title;
            button.FontSize = 14;
            button.Click += (object sender, RoutedEventArgs e) =>
            {
                Logging.Log($"{InstanceName} {button.Content} button clicked!");
                eventButtonClicked?.Execute();
            };
            button.IsEnabled = enabled;
        }

        // IUI implementation ------------------------------------------------------
        UIElement IUI.GetWPFElement()
        {
            return button;
        }

        // IDataFlow<bool> implementation
        bool IDataFlow<bool>.Data { set => button.IsEnabled = value; }
    }
}
