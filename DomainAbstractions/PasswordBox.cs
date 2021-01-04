using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgrammingParadigms;

namespace DomainAbstractions
{
    /// <summary>
    /// <para>Contains a WPF PasswordBox and both implements and provides ports for setting/getting the text inside.</para>
    /// <para>Ports:</para>
    /// <para>1. IUI wpfElement: returns the contained PasswordBox</para>
    /// <para>2. IDataFlow&lt;string&gt; content: The string contained in the PasswordBox</para>
    /// <para>3. IDataFlowB&lt;string&gt; returnContent: returns the string contained in the PasswordBox</para>
    /// <para>4. IEvent clear: clears the text content inside the PasswordBox</para>
    /// <para>5. IDataFlow&lt;string&gt; textOutput: outputs the string contained in the PasswordBox</para>
    /// </summary>
    public class PasswordBox : IUI, IDataFlow<string>, IDataFlowB<string>, IEvent
    {
        // properties
        public string InstanceName = "Default";

        // PasswordBox overlaps with Systems.Windows.Controls.PasswordBox if we have "using System.Windows.Controls;"
        private System.Windows.Controls.PasswordBox passwordBox = new System.Windows.Controls.PasswordBox();

        /// <summary>
        /// <para>Contains a WPF PasswordBox and both implements and provides ports for setting/getting the text inside.</para>
        /// </summary>
        public PasswordBox() 
        {
            passwordBox.PasswordChanged += (object sender, System.Windows.RoutedEventArgs e) => PasswordBox_PasswordChanged();
        }

        private void PasswordBox_PasswordChanged()
        {
            password = passwordBox.Password;
            if (textOutput != null) textOutput.Data = password;
            DataChanged?.Invoke();
        }
        
        public string Password
        {
            set
            {
                passwordBox.Password = value;
                password = value;
            }
        }

        // Fields
        private string password;

        // Output
        private IDataFlow<string> textOutput;

        // IUI implementation
        System.Windows.UIElement IUI.GetWPFElement()
        {
            return passwordBox;
        }

        // IDataFlow<string> implementation
        string IDataFlow<string>.Data { set => passwordBox.Password = value; }

        // IDataFlowB<string> implementation
        public event DataChangedDelegate DataChanged;

        string IDataFlowB<string>.Data
        {
            get { Debug.WriteLine("Password sent!"); return password; }
        }

        // IEvent implementation
        void IEvent.Execute()
        {
            passwordBox.Clear();
        }

    }
}
