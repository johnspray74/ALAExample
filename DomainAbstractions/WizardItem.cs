using ProgrammingParadigms;
using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace DomainAbstractions
{
    /// <summary>
    /// One of the radio buttons of a Wizard.
    /// 
    /// Boolean data output that is true for the selected radio button and false for the rest, and an event. 
    /// Goes false when the wizard is cancelled or the whole operation completes.
    /// </summary>
    public class WizardItem : IUIWizard, IDataFlow<bool>, IDataFlow<string>
    {
        // properties
        public string InstanceName = "Default";
        public string ContentText { set => checkRadioButton.Content = value; }
        public string ImageName { set => imageView.Source = new BitmapImage(new Uri(@"pack://application:,,,/" + Assembly.GetExecutingAssembly().GetName().Name + @";component/Application/" + string.Format("Resources/{0}", value), UriKind.Absolute)); }
        public bool Checked { get => (bool)checkRadioButton.IsChecked; set => checkRadioButton.IsChecked = value; }
        public bool Visible { set => SetVisibility(value); }

        // outputs
        private IEvent eventOutput;

        // private fields
        private Image imageView;
        private System.Windows.Controls.RadioButton checkRadioButton;

        /// <summary>
        /// An IUI element that only exists in a Wizard.
        /// </summary>
        /// <param name="contentText">the text displayed on the item</param>
        public WizardItem(string contentText = null)
        {
            imageView = new Image();
            checkRadioButton = new System.Windows.Controls.RadioButton();
            checkRadioButton.Content = contentText;
        }

        /// <summary>
        /// Sets the visibility of the wizard item.
        /// </summary>
        /// <param name="visible">Whether the wizard item is visible.</param>
        private void SetVisibility(bool visible)
        {
            Visibility vis = visible ? Visibility.Visible : Visibility.Collapsed;
            imageView.Visibility = vis;
            checkRadioButton.Visibility = vis;
        }

        // IUI implementation ------------------------------------------------------
        UIElement IUI.GetWPFElement()
        {
            StackPanel contentPanel = new StackPanel();
            contentPanel.Orientation = Orientation.Horizontal;
            contentPanel.Height = 48;

            imageView.Width = 38;
            imageView.Height = 38;
            imageView.VerticalAlignment = VerticalAlignment.Center;
            imageView.Margin = new Thickness(10, 0, 6, 0);

            checkRadioButton.VerticalAlignment = VerticalAlignment.Center;
            checkRadioButton.Margin = new Thickness(6, 0, 6, 0);
            checkRadioButton.GroupName = "groupName";
            checkRadioButton.FontSize = 17;

            contentPanel.Children.Add(imageView);
            contentPanel.Children.Add(checkRadioButton);

            //try
            //{

            //}
            //catch(System.InvalidOperationException e)
            //{
            //    DependencyObject wizardParent = VisualTreeHelper.GetParent(contentPanel);

            //}
            //if((VisualTreeHelper.GetParent as IUI).GetWPFElement.contentPanel.Children.)
            return contentPanel;
        }


        // IUIWizard implementation -------------------------------------------------
        bool IUIWizard.Checked => Checked;

        void IUIWizard.GenerateOutputEvent()
        {
            eventOutput?.Execute();
        }

        // IDataFlow<bool> implementation
        bool IDataFlow<bool>.Data { set => SetVisibility(value); }

        // IDataFlow<string> implementation
        string IDataFlow<string>.Data { set => checkRadioButton.Content = value; }
    }
}
