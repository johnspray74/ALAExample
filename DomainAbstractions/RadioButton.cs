using ProgrammingParadigms;
using System.Windows;

namespace DomainAbstractions
{
    /// <summary>
    /// This abstraction is very similiar to Button abstraction but of a radio button
    /// which can be customized by it's Title, Width, Height, FontSize and Margin. 
    /// It is a selectable UI element which generates an IEvent when selected and confirmed.
    /// ------------------------------------------------------------------------------------------------------------------
    /// Ports:
    /// IUI "inputIUI": input IUI to get the WPF element
    /// IDataFlow<bool> "selected": Whether the radio button is pressed.
    /// IEvent "eventButtonClicked": output event when radio button is clicked
    /// </summary>
    public class RadioButton : IUI, IPersistable
    {
        // properties ------------------------------------------------------------
        public string InstanceName = "Default";

        // properties for customizing UI
        public double Width { set => radioButton.Width = value; }
        public double Height { set => radioButton.Height = value; }
        public double FontSize { set => radioButton.FontSize = value; }
        public Thickness Margin { set => radioButton.Margin = value; }
        public bool Checked { set => radioButton.IsChecked = value; }
        public bool Visible { set => radioButton.Visibility = value ? Visibility.Visible : Visibility.Collapsed; }

        public string ToolTip;

        // outputs ---------------------------------------------------------------
        private IEvent eventButtonClicked;
        private IDataFlow<bool> selected;
        private IDataFlowB<bool> isVisible;
        private IDataFlowB<bool> isChecked;

        // private fields --------------------------------------------------------
        private System.Windows.Controls.RadioButton radioButton;
        private bool buttonVisible = true;
        private bool buttonChecked = false;

        /// <summary>
        /// A selectable UI element which omits an IEvent output when selected and confirmed.
        /// It can be customized by setting the Title, Width, Height FontSize and Margin.
        /// </summary>
        /// <param name="title">The text displayed on the radioButton</param>
        public RadioButton(string title)
        {
            radioButton = new System.Windows.Controls.RadioButton();
            radioButton.Content = title;
            radioButton.FontSize = 16;
            radioButton.ToolTip = ToolTip;
            radioButton.Margin = new Thickness(5);
            radioButton.Checked += (object sender, RoutedEventArgs e) =>
            {
                if (selected != null) selected.Data = true;
                Selected?.Invoke(true);
                eventButtonClicked?.Execute();
            };
            radioButton.Unchecked += (object sender, RoutedEventArgs e) =>
            {
                if (selected != null) selected.Data = false;
                Selected?.Invoke(false);
            };
            RefreshRadioButton();
        }

        // IUI implementation ------------------------------------------------------
        UIElement IUI.GetWPFElement()
        {
            return radioButton;
        }

        // IPersistable implementation
        object IPersistable.Key { get => radioButton.Content; }
        void IPersistable.SetState(bool selected) => radioButton.Dispatcher.Invoke(() => radioButton.IsChecked = selected);
        public event PersistableDelegate Selected;

        private void PostWiringInitialize()
        {
            if (isVisible != null) isVisible.DataChanged += () =>
            {
                buttonVisible = isVisible.Data;
                RefreshRadioButton();
            };

            if (isChecked != null) isChecked.DataChanged += () =>
            {
                buttonChecked = isChecked.Data;
                RefreshRadioButton();
            };

            if (selected != null) selected.Data = (bool)radioButton.IsChecked;
        }

        private void RefreshRadioButton()
        {
            radioButton.Visibility = buttonVisible ? Visibility.Visible : Visibility.Collapsed;
            radioButton.IsChecked = buttonChecked;
        }
    }
}

