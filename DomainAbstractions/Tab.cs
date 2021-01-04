﻿using ProgrammingParadigms;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows;

namespace DomainAbstractions
{
    /// <summary>
    /// This abstraction is a UI element of TabItem and StackPanel to display the Header and content of each tab.
    /// Title for header, FontSize for header, Height and Margin of the panel for the content can be customized.
    /// ------------------------------------------------------------------------------------------------------------------
    /// Ports:
    /// IUI "inputIUI": input IUI to get the WPF element
    /// List<IUI> "tabItemList": input IUI to get the WPF element
    /// </summary>
    public class Tab: IUI
    {
        // outputs
        private List<IUI> tabItemList = new List<IUI>();

        // properties
        public string InstanceName = "Default";
        public double FontSize { set => tabItem.FontSize = value; }
        public double Height { set => stackPanel.Height = value; }
        public Thickness Margin { set => stackPanel.Margin = value; }

        // private fields
        private TabItem tabItem;
        private StackPanel stackPanel;

        /// <summary>
        /// UI element that stores the content of each tab
        /// </summary>
        /// <param name="title">The name displayed on the tag page</param>
        public Tab(string title)
        {
            tabItem = new TabItem();
            tabItem.Header = title;
            stackPanel = new StackPanel() { Margin = new Thickness(10) };
            tabItem.Content = stackPanel;
        }

        // IUI implementation ------------------------------------------------------
        // Adds all the IU content into the tab panel
        UIElement IUI.GetWPFElement()
        {
            foreach (var c in tabItemList) stackPanel.Children.Add(c.GetWPFElement());
            return tabItem;
        }
    }
}
