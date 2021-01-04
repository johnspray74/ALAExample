using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgrammingParadigms;
using System.Windows;
using System.Windows.Controls;

namespace DomainAbstractions
{
    /// <summary>
    /// <para>Contains a WPF CheckBox that outputs its checked/unchecked status and an event when it is checked.</para>
    /// <para>Ports:</para>
    /// <para>1. IUI wpfElement: returns the contained WPF CheckBox.</para>
    /// <para>2. IPersistable persistance: communicates state persistance.</para>
    /// <para>2. IDataFlow&lt;string&gt; content: Sets the text label next to the checkbox.</para>
    /// <para>3. IDataFlow&lt;bool&gt; isChecked: Outputs whether or not the checkbox is checked.</para>
    /// <para>4. IEvent boxChecked: Fires an event when the checkbox is checked.</para>
    /// </summary>
    public class CheckBox : IUI, IPersistable, IDataFlow<string>
    {
        // properties ---------------------------------------------------------------
        public string InstanceName = "Default";
        public HorizontalAlignment horizontalAlignment { set => checkBox.HorizontalAlignment = value; }
        public Thickness Margin { set => checkBox.Margin = value; }
        public double Height { set => checkBox.Height = value; }
        public double FontSize { set => checkBox.FontSize = value; }
        public string Text { set => checkBox.Content = value; }

        // Ports ---------------------------------------------------------------
        private IDataFlow<bool> isChecked;
        private IEvent boxChecked;

        // Fields ---------------------------------------------------------------
        private System.Windows.Controls.CheckBox checkBox = new System.Windows.Controls.CheckBox();

        /// <summary>
        /// <para>Contains a WPF CheckBox that outputs its checked/unchecked status and an event when it is checked.</para>
        /// </summary>
        /// <param name="text"></param>
        /// <param name="checkedByDefault"></param>
        public CheckBox(string text = "", bool checkedByDefault = false)
        {
            checkBox.Content = text;
            if (checkedByDefault) checkBox.IsChecked = true;
            checkBox.Click += CheckBox_Checked;
        }

        void PostWiringInitialize()
        {
            if (isChecked != null) isChecked.Data = (bool)checkBox.IsChecked;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (isChecked != null) isChecked.Data = (bool)checkBox.IsChecked; // Need to cast as a bool because checkBox.IsChecked is of type "bool?"

            Selected?.Invoke((bool)checkBox.IsChecked);
            boxChecked?.Execute();
        }


        // IUI implementation
        System.Windows.UIElement IUI.GetWPFElement()
        {
            return checkBox;
        }

        // IPersistable implementation
        object IPersistable.Key { get => checkBox.Content; }
        void IPersistable.SetState(bool selected) => checkBox.Dispatcher.Invoke(() => checkBox.IsChecked = selected);
        public event PersistableDelegate Selected;

        // IDataFlow<string> implementation
        string IDataFlow<string>.Data { set => checkBox.Content = value; }
    }
}
