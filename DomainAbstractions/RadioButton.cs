using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using ProgrammingParadigms;

namespace DomainAbstractions
{
    /// <summary>
    /// Radio button is an ALA domain abstraction
    /// Abstraction description follows:
    /// Radio button is typically one of a set of radio buttons. They are mutually excluive to each other - the user selects one.
    /// When the parent UI element sends us an event that selection is complete, the radio button that is selected outputs an event.
    /// ------------------------------------------------------------------------------------------------------------------
    /// Configurations: (configurations are for use by the application when it instantiates a domain abstraction)
    ///     text (parameter of constructor) configures the text that display to the right of the radio button.
    ///     InstanceName property: As with all domain abstractions, we have an instance name. (Because there can be multiple instances of this abstraction, the application gives us an object name which is not generally used by the abstraction internal logic. It is only used during debugging so you can tell which object you are break-pointed on.
    ///     ImageName property: Allows the application to specify an image that displays to the left of the radio button as an icon.
    ///     Checked: Allows the application to set which radio button is initially selected.
    /// ------------------------------------------------------------------------------------------------------------------
    /// Ports:
    ///     parent (IUISet): for wiring the radio button to its parent UI container.
    ///     visible (IDataFlow<bool>): boolean input that controls the visibilty of the radio button. If false, the radio button is collapsed, not greyed out.
    ///     output (IEvent): outputs an event if the radio button is selected when the parent UI container tells us that the user has finished his selection (for example, if the parent is a Wizard, the Wizard will send an event to us when the wizard's NEXT button is pressed, and we will in turn output an event on this port if we have been selected by the user).
    /// Future generalization:
    ///     Implement IUI as well. If wired to a parent by IUI instead of IUISet, then we output an event immediately when the radio button is selected instead of waiting for the parent to signal via the Event in IUISet.
    /// </summary>
    public class RadioButton : IUISet, IDataFlow<bool> // UIparent, visible
    {
        // properties
        public string InstanceName { get; set; } = "Default";
        public string ImageName { set => imageView.Source = new BitmapImage(new Uri(@"pack://application:,,,/" + Assembly.GetExecutingAssembly().GetName().Name + @";component/Application/" + string.Format("Resources/{0}", value), UriKind.Absolute)); }
    
        // ports
        private IEvent output;

        // private fields
        private Image imageView;
        private System.Windows.Controls.RadioButton checkRadioButton;
        private bool Checked { get => (bool)checkRadioButton.IsChecked; set => checkRadioButton.IsChecked = value; }

        /// <summary>
        /// An IUI element that only exists in a Wizard.
        /// </summary>
        /// <param name="contentText">the text displayed on the item</param>
        public RadioButton(string text = null)
        {
            imageView = new Image();
            checkRadioButton = new System.Windows.Controls.RadioButton();
            checkRadioButton.Content = text;
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

            return contentPanel;
        }


        // IUISet implementation -------------------------------------------------

        void IUISet.Event()
        {
            if (Checked) output?.Execute();
        }

        // IDataFlow<bool> implementation
        bool IDataFlow<bool>.Data { set => SetVisibility(value); }



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

    }
}
