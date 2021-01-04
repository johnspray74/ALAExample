using ProgrammingParadigms;
using System.Collections.Generic;
using System.Windows;

namespace DomainAbstractions
{
    /// <summary>
    /// Menubar as found on most applications. Displays Menu's horizontally. 
    /// Implements IUI. Has a list of Menus which are IUI.
    /// </summary>
    public class Menubar : IUI
    {
        // properties
        public string InstanceName = "Default";

        // outputs
        private List<IUI> children = new List<IUI>();

        // private fields
        private System.Windows.Controls.Menu menuBar = new System.Windows.Controls.Menu();

        /// <summary>
        /// An IUI element which is a windows-desktop application style MenuBar. 
        /// </summary>
        public Menubar() { }

        // IUI implementation ----------------------------------------------------
        UIElement IUI.GetWPFElement()
        {
            foreach (var c in children) menuBar.Items.Add(c.GetWPFElement());
            return menuBar;
        }
    }
}
