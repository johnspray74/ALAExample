using ProgrammingParadigms;
using System;
using System.Windows;
using System.Windows.Media;

namespace DomainAbstractions
{
    /// <summary>
    /// Generally, it is a window-style progress bar. Resizable depend on the parent container who owns it.
    /// ------------------------------------------------------------------------------------------------------------------
    /// Ports:
    /// 1. IUI "NEEDNAME"
    /// 2. IDataFlowB<string> "progressValue": for displaying the progress
    /// 3. IDataFlowB<string> "maximumValue": for maximum amount
    /// </summary>
    public class ProgressBar : IUI
    {
        // properties
        public string InstanceName = "Default";

        // outputs - actually it's a reversal output which used for input progress data
        private IDataFlowB<string> progressValue;
        private IDataFlowB<string> maximumValue;

        // private fields
        private System.Windows.Controls.ProgressBar progressBar;

        /// <summary>
        /// An IUI abstraction for displaying windows-style progress bar with a current and maximum input.
        /// </summary>
        public ProgressBar(double maximum = 1)
        {            
            progressBar = new System.Windows.Controls.ProgressBar();
            progressBar.Margin = new Thickness(5);
            progressBar.Height = 30;
            progressBar.Visibility = Visibility.Visible;
            progressBar.Maximum = maximum;
            progressBar.Loaded += (object sender, RoutedEventArgs e) =>
            {
                progressBar.ApplyTemplate();
                System.Windows.Controls.Panel p = progressBar.Template.FindName("Animation", progressBar) as System.Windows.Controls.Panel;
                // Arnab: NullPointerException when the following line is uncommented
                //p.Background = new SolidColorBrush(Color.FromRgb(21, 193, 64));
            };
        }

        // binding the events
        private void PostWiringInitialize()
        {
            if (progressValue != null)
            {
                progressValue.DataChanged += () =>
                {
                    progressBar.Value = Convert.ToInt32(progressValue.Data);
                };
            }

            if (maximumValue != null)
            {
                maximumValue.DataChanged += () =>
                {
                    progressBar.Maximum = Convert.ToInt32(maximumValue.Data);
                };
            }
        }

        // IUI implmentation -----------------------------------------------------
        UIElement IUI.GetWPFElement()
        {
            return progressBar;
        }
    }
}
