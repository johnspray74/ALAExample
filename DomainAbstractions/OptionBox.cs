using ProgrammingParadigms;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace DomainAbstractions
{
    /// <summary>
    /// A UI element that presents a dropdown list of options.
    /// The set of options are a list of OptionsBoxItems.
    /// This list is type IDataFlow with a string type, so the OptionBox can directly read the string values of the options.
    /// OptionBox itself has two output ports of type IDataFlow:
    /// - The string value of the selected option.
    /// - the index number of the selected option.
    /// The OptionBoxItem has a IDataFlow output port of type boolean, which outputs true while that option is selected, and emits an event when first selected.
    /// This provides a convenient way of getting separate boolean outputs for each option when they are needed for example to control gates.
    /// The OptionBox sends true/false events to the OptionBoxItems to tell them they are selected or not selected.
    /// an unresolved problem is that IDataFlow interface does not support these events, so another interface type is needed between the OptionBox and the OptionBoxItem.
    /// All the other similar pair situations such as MenuBar/Menu, Menu/MenuItem, TooBar/Tool, RadioButtons/RadioButtonItem, Tab/Tab,
    /// Wizard/WizardItem also have restricted grammar(must be used in pairs, even though other IUI types may actually work.)
    /// So they perhaps should all be different interfaces based on IUI, even if they are functionally identical to IUI.Every one of these has a concept of either
    /// being in focus or being selected. So the OptionBox/OptionBoxItem pair seems like an unusual case because it doesn't use the IUI interface.
    /// </summary>
    public class OptionBox : IUI, IDataFlow<bool>
    {
        // properties
        public string InstanceName = "Default";
        public string DefaultTitle;

        // outputs
        private IDataFlow<string> dataFlowSelectionOutput;
        private IDataFlow<int> dataFlowSelectionIndex;
        private List<IUI> children = new List<IUI>();
         
        // private fields
        private ComboBox comboBox = new ComboBox();
        private ComboBoxItem defaultItem = new ComboBoxItem() { IsSelected = true };  // placeholder of the default title

        /// <summary>
        /// An IUI dropdown list box which displays a list of OptionBoxItems when clicked.
        /// </summary>
        /// <param name="Title">text place holder of the option box</param>
        public OptionBox(string Title = "", int height = 24, int width = 110, bool visible = true)
        {
            comboBox.Height = height;
            comboBox.Width = width;
            comboBox.DropDownOpened += ComboBoxDropDownOpened;
            comboBox.DropDownClosed += ComboBoxDropDownClosed;
            comboBox.SelectionChanged += ComboBoxSelectionChanged;
            comboBox.Items.Add(defaultItem);
            comboBox.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
        }

        // IUI implmentation -----------------------------------------------------------------
        UIElement IUI.GetWPFElement()
        {
            defaultItem.Content = DefaultTitle;
            if (dataFlowSelectionOutput != null) dataFlowSelectionOutput.Data = DefaultTitle;

            foreach (var child in children)
            {
                comboBox.Items.Add(child.GetWPFElement());
            }
            return comboBox;
        }


        // private method - drop down event actions -----------------------------------------------------------
        private void ComboBoxDropDownOpened(object sender, EventArgs e) => comboBox.Items.Remove(defaultItem);

        private void ComboBoxDropDownClosed(object sender, EventArgs e)
        {
            if (comboBox.SelectedItem == null)
            {
                defaultItem.IsSelected = true;
                comboBox.Items.Add(defaultItem);
            }            
        }

        private void ComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBox.SelectedItem != null)
            {
                string selection = (comboBox.SelectedItem as ComboBoxItem).Content as string;

                if (dataFlowSelectionOutput != null)
                {
                    dataFlowSelectionOutput.Data = selection;
                }

                if (dataFlowSelectionIndex != null)
                {
                    dataFlowSelectionIndex.Data = comboBox.Items.IndexOf(comboBox.SelectedItem);
                }
            }          
        }

        // IDataFlow<bool> implementation
        bool IDataFlow<bool>.Data { set => comboBox.Visibility = value ? Visibility.Visible : Visibility.Collapsed; }
    }
}
