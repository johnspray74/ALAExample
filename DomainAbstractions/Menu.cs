using ProgrammingParadigms;
using System.Collections.Generic;
using System.Windows;

namespace DomainAbstractions
{
    /// <summary>
    /// One Menu that sits on a Menubar. Has a list of MenuItems which are IUIs Displays MenuItems vertically 
    /// when the menu is clicked. Implements IUI interface.
    /// </summary>
    public class Menu : IUI
    {
        // properties
        public string InstanceName = "Default";
        public double FontSize { set; get; } = 14;

        // outputs
        private List<IUI> children = new List<IUI>();

        // private fields
        private System.Windows.Controls.MenuItem menu = new System.Windows.Controls.MenuItem();

        /// <summary>
        /// An IUI element which is a windows-desktop application style Menu. 
        /// </summary>
        /// <param name="title">The title to display</param>
        public Menu(string title)
        {
            menu.Header = title;
        }

        // IUI implementation ---------------------------------------------------
        UIElement IUI.GetWPFElement()
        {
            menu.FontSize = FontSize;
            foreach (var c in children) menu.Items.Add(c.GetWPFElement());
            return menu;
        }
    }
}
