using ProgrammingParadigms;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DomainAbstractions
{
    /// <summary>
    /// A toolbar typically has tool icon pictures laid out horizontally.
    /// Implements IUI
    /// Has a list of Tools which are IUIs.
    /// </summary>
    public class Toolbar : IUI
    {
        // properties
        public string InstanceName = "Default";
        public Thickness Margin { set => toolBarPanel.Margin = value; }

        // ports
        private List<IUI> children = new List<IUI>();

        // private fields
        private DockPanel toolBarPanel = new DockPanel();

        /// <summary>
        /// An IUI abstraction displays a list of tool items horizontally.
        /// </summary>
        public Toolbar()
        {
            toolBarPanel.HorizontalAlignment = HorizontalAlignment.Left;
            toolBarPanel.Background = new SolidColorBrush(Color.FromRgb(240, 240, 240));
            toolBarPanel.Height = 65;
        }

        // IUI implementation --------------------------------------------------------
        public UIElement GetWPFElement()
        {
            foreach (var c in children)
            {
                toolBarPanel.Children.Add(c.GetWPFElement());
            }
            return toolBarPanel;
        }
    }
}
