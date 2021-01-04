using ProgrammingParadigms;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace DomainAbstractions
{
    /// <summary>
    /// This is a specific IUI abstraction, it does not present any concrete, but is used for organizing the 
    /// exhibition of the children. It works similar as Horizontal, Vertical, etc.
    /// </summary>
    public class RightJustify : IUI
    {
        // properties
        public string InstanceName = "Default";
        public Thickness Margin { set => backgroundPanel.Margin = value; }
        public VerticalAlignment VertAlignment { set => backgroundPanel.VerticalAlignment = value; }

        // outputs
        private List<IUI> children = new List<IUI>();

        // private fields
        private StackPanel backgroundPanel = new StackPanel();

        /// <summary>
        /// An IUI element which layout it's sub-elements at the right side.
        /// </summary>
        public RightJustify()
        {
            backgroundPanel.Orientation = Orientation.Horizontal;
            backgroundPanel.HorizontalAlignment = HorizontalAlignment.Right;
            backgroundPanel.VerticalAlignment = VerticalAlignment.Center;
        }

        // IUI implementation ------------------------------------------------------
        UIElement IUI.GetWPFElement()
        {
            foreach (var child in children)
            {
                backgroundPanel.Children.Add(child.GetWPFElement());
            }
            return backgroundPanel;
        }
    }
}
