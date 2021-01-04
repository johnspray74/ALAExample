using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ProgrammingParadigms;

namespace DomainAbstractions
{
    /// <summary>
    /// A simple IUI abstraction which dispalys a LiteralString as a status.
    /// It displays some text status by a giving LiteralString. Text color can be customized.
    /// ------------------------------------------------------------------------------------------------------------------
    /// Ports:
    /// IUI "NEEDNAME": The input IUI for get WPF element.
    /// List<IUI> "children": 
    /// </summary>
    public class Statusbar : IUI
    {
        // properties
        public string InstanceName = "Default";

        // outputs
        private List<IUI> children = new List<IUI>();

        // private fields
        private System.Windows.Controls.Primitives.StatusBar statusBar = new System.Windows.Controls.Primitives.StatusBar();
        private TextBlock statusTextBlock = new TextBlock();

        /// <summary>
        /// It displays some text status by a giving LiteralString. Text color can be customized.
        /// </summary>
        public Statusbar()
        {
            statusBar.Height = 25;
            statusBar.BorderThickness = new Thickness(0, 1, 0, 0);
            statusBar.BorderBrush = new SolidColorBrush(Color.FromRgb(200, 200, 200));
        }

        // IUI implementation -----------------------------------------------------------
        UIElement IUI.GetWPFElement()
        {
            foreach (var c in children) statusBar.Items.Add(c.GetWPFElement());
            return statusBar;
        }
    }
}
