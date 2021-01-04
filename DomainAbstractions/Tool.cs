using ProgrammingParadigms;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DomainAbstractions
{
    /// <summary>
    /// A tool used by the the ToolBar.
    /// Implements IUI.
    /// When clicked, generates an event on its RHS port.
    /// </summary>
    public class Tool : IUI, IDataFlow<bool>
    {
        // properties
        public string InstanceName = "Default";
        public string ToolTip;

        // outputs
        private IEvent eventOutput;
        private IUI iuiStructure;
        private IDataFlowB<bool> dataFlowBOutput;

        // private fields
        private System.Windows.Controls.Button toolButton = new System.Windows.Controls.Button();

        /// <summary>
        /// An IUI abstraction used by Toolbar, generates IEvent when clicked.
        /// </summary>
        /// <param name="ImageName">the image displayed on the tool</param>
        public Tool(string ImageName, bool visible = true)
        {
            toolButton.Margin = new Thickness(5, 5, 5, 5);
            toolButton.Background = new ImageBrush() { ImageSource = new BitmapImage(new System.Uri(@"pack://application:,,,/" + Assembly.GetExecutingAssembly().GetName().Name + @";component/Application/" + string.Format("Resources/{0}", ImageName), System.UriKind.Absolute)) };
            toolButton.Width = 45;
            toolButton.Height = 45;
            toolButton.Click += ButtonClicked;
            toolButton.Style = GetButtonStyle();
            toolButton.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
        }

        private void PostWiringInitialize()
        {
            if (dataFlowBOutput != null)
            {
                dataFlowBOutput.DataChanged += () =>
                {
                    toolButton.IsEnabled = dataFlowBOutput.Data;
                    toolButton.Foreground = toolButton.IsEnabled ? new SolidColorBrush(Color.FromRgb(0, 0, 0)) : Brushes.DarkGray;
                };
            }
        }

        // IUI implementation -----------------------------------------------------------
        UIElement IUI.GetWPFElement()
        {
            iuiStructure?.GetWPFElement();

            toolButton.ToolTip = ToolTip;
            return toolButton;
        }

        // IDataFlow<bool> implementation -----------------------------------------------
        bool IDataFlow<bool>.Data { set => toolButton.Visibility = value ? Visibility.Visible : Visibility.Collapsed; }

        private void ButtonClicked(object sender, RoutedEventArgs e) => eventOutput?.Execute();

        private Style GetButtonStyle()
        {
            var buttonStyle = new Style();
            buttonStyle.Setters.Add(new Setter() { Property = FrameworkElement.VerticalAlignmentProperty, Value = VerticalAlignment.Stretch });
            buttonStyle.Setters.Add(new Setter() { Property = FrameworkElement.HorizontalAlignmentProperty, Value = HorizontalAlignment.Stretch });
            buttonStyle.Setters.Add(new Setter() { Property = Control.BorderBrushProperty, Value = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0)) });
            buttonStyle.Setters.Add(new Setter() { Property = Control.BorderThicknessProperty, Value = new Thickness(0) });

            // cell selection style
            buttonStyle.Resources.Add(SystemColors.InactiveSelectionHighlightBrushKey, Brushes.DodgerBlue);
            var trigger = new Trigger() { Property = UIElement.IsMouseOverProperty, Value = true };
            trigger.Setters.Add(new Setter(Control.BackgroundProperty, Brushes.DodgerBlue));
            buttonStyle.Triggers.Add(trigger);

            return buttonStyle;
        }
    }
}
