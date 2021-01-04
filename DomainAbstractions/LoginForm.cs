using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ProgrammingParadigms;

namespace DomainAbstractions
{
    public class LoginForm : IUI
    {
        // Properties
        public string InstanceName = "Default";
        public Thickness Margin { set => stackPanel.Margin = value; }

        private StackPanel stackPanel = new StackPanel();


        // Outputs
        List<IUI> children = new List<IUI>();

        // IUI implementation
        System.Windows.UIElement IUI.GetWPFElement()
        {
            foreach (var c in children) stackPanel.Children.Add(c.GetWPFElement());

            return stackPanel;
        }
    }
}
