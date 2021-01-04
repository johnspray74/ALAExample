using ProgrammingParadigms;
using System.IO.Ports;
using System.Windows;
using System.Windows.Controls;

namespace DomainAbstractions
{
    /// <summary>
    /// A drop down list of COM ports. Fires an IEvent when the selection changes and an IDataFlow<string> is
    /// updated with the current selection.
    /// -----------------------------------------------------------------------------------------------------
    /// Ports:
    /// 1. IUI NEEDNAME:                    IUI hook
    /// 2. IEvent NEEDNAME:                 Refreshes the COM port list.
    /// 3. IDataFlow<string> selectionName: The current selection, or null.
    /// 4. IEvent selectionChanged:         Fired when the selection changes.
    /// </summary>
    class ComPortOptionBox : IUI, IEvent
    {
        // properties
        public string InstanceName = "Default";

        // ports
        private IDataFlow<string> selectionName;
        private IEvent selectionChanged;

        // private fields
        private ComboBox comboBox = new ComboBox();
        private string[] comPorts = new string[] { };

        /// <summary>
        /// A drop down list of COM ports. Fires an IEvent when the selection changes and an IDataFlow<string> is
        /// updated with the current selection.
        /// </summary>
        /// <param name="height">Combo box height</param>
        /// <param name="width">Combo box width</param>
        /// <param name="visible">Combo box visibility</param>
        public ComPortOptionBox(int height = 24, int width = 110, bool visible = true)
        {
            comboBox.Height = height;
            comboBox.Width = width;
            comboBox.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;

            comboBox.SelectionChanged += ComboBox_SelectionChanged;
        }

        // comboBox event listeners
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (selectionName != null)
            {
                if (comboBox.SelectedItem != null)
                {
                    selectionName.Data = (comboBox.SelectedItem as ComboBoxItem).Content as string;
                }
                else
                {
                    selectionName.Data = null;
                }
            }

            selectionChanged?.Execute();
        }

        /// <summary>
        /// Fetches the list of COM ports and updates the combo box.
        /// </summary>
        private void Fetch()
        {
            // attempt to get com ports - if it fails,
            // push the old list
            try
            {
                comPorts = SerialPort.GetPortNames();
            }
            catch
            { }

            // add com ports back to list
            comboBox.Items.Clear();
            for (int i = 0; i < comPorts.Length; i++)
            {
                comboBox.Items.Add(new ComboBoxItem() { Content = comPorts[i], IsSelected = i == 0 ? true : false });
            }
        }

        // IUI implementation
        UIElement IUI.GetWPFElement() => comboBox;

        // IEvent implementation
        void IEvent.Execute() => Fetch();
    }
}
