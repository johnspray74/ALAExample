using ProgrammingParadigms;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace DomainAbstractions
{
    /// <summary>
    /// This abstraction is a UI element of GroupBox which can be a containment for IUI elements e.g. Tabs.
    /// FontSize can be specificied for customisation.
    /// ------------------------------------------------------------------------------------------------------------------
    /// Ports:
    /// 1. IUI inputIUI: input IUI to get the WPF element
    /// 2. IDataFlow<bool> visibile: Sets the visibility of the group box.
    /// 3. IDataFlow<string> title: Sets the title of the group box.
    /// 4. List<IUI> groupBoxContent: output list of IU elements contained in this GroupingBox
    /// </summary>
    public class GroupingBox: IUI, IDataFlow<bool>, IDataFlow<string> // inputIUI, visibile, title
    {
        // properties ---------------------------------------------------------------------
        public string InstanceName = "Default";
        public double FontSize { set => groupBox.FontSize = value; }

        // ports ---------------------------------------------------------------------
        private List<IUI> groupBoxContent = new List<IUI>();

        //private fields ---------------------------------------------------------------------
        private GroupBox groupBox;
        private StackPanel stackPanel;

        /// <summary>
        /// This abstraction is a UI element of GroupBox which can be a containment for IUI elements e.g. Tabs.
        /// </summary>
        public GroupingBox(string title, bool visible = true)
        {
            groupBox = new GroupBox();
            stackPanel = new StackPanel();
            groupBox.Content = stackPanel;
            groupBox.Header = title;
            SetVisiblity(visible);
        }

        /// <summary>
        /// Sets the visibility of the grouping box.
        /// </summary>
        /// <param name="visibility">The visibility</param>
        private void SetVisiblity(bool visibility)
        {
            groupBox.Visibility = visibility ? Visibility.Visible : Visibility.Collapsed;
        }

        // IUI implementation ------------------------------------------------------
        // Adds all the content into the groupbox
        UIElement IUI.GetWPFElement()
        {
            foreach (var c in groupBoxContent) stackPanel.Children.Add(c.GetWPFElement());
            return groupBox;
        }

        // IDataFlow<bool> implementation
        bool IDataFlow<bool>.Data { set => SetVisiblity(value); }

        // IDataFlow<string> implementation
        string IDataFlow<string>.Data { set => groupBox.Header = value; }
    }
}

