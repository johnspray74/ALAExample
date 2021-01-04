using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgrammingParadigms;
using System.Windows;

namespace DomainAbstractions
{
    /// <summary>
    /// <para>Contains a WPF TextBox and both implements and provides ports for setting/getting the text inside.</para>
    /// <para>Ports:</para>
    /// <para>1. IUI wpfElement: returns the contained TextBox</para>
    /// <para>2. IDataFlow&lt;string&gt; content: The string contained in the TextBox</para>
    /// <para>3. IDataFlowB&lt;string&gt; returnContent: returns the string contained in the TextBox</para>
    /// <para>4. IEvent clear: clears the text content inside the TextBox</para>
    /// <para>5. IDataFlow&lt;string&gt; textOutput: outputs the string contained in the TextBox</para>
    /// </summary>
    public class TextBox : IUI, IDataFlow<string>, IDataFlowB<string>, IEvent
    {
        // properties
        public string InstanceName = "Default";
        public HorizontalAlignment horizontalAlignment { set => textBox.HorizontalAlignment = value; }
        public Thickness Margin { set => textBox.Margin = value; }
        public double Height { set => textBox.Height = value; }
        public double FontSize { set => textBox.FontSize = value; }
        public string Text
        {
            set
            {
                textBox.Text = value;
                text = value;
            }
        }

        // TextBox overlaps with Systems.Windows.Controls.TextBox if we have "using System.Windows.Controls;"
        private System.Windows.Controls.TextBox textBox = new System.Windows.Controls.TextBox();

        // Fields
        private string text;

        // Outputs
        private IDataFlow<string> textOutput;

        /// <summary>
        /// <para>Contains a WPF TextBox and both implements and provides ports for setting/getting the text inside.</para>
        /// </summary>
        public TextBox(bool readOnly = false)
        {
            textBox.TextChanged += (object sender, System.Windows.Controls.TextChangedEventArgs e) => TextBox_TextChanged();
            //DataChanged = TextBox_TextChanged;
            textBox.HorizontalScrollBarVisibility = System.Windows.Controls.ScrollBarVisibility.Auto;
            textBox.VerticalScrollBarVisibility = System.Windows.Controls.ScrollBarVisibility.Auto;
            textBox.IsReadOnly = readOnly;
        }

        private void TextBox_TextChanged()
        {
            text = textBox.Text;
            if (textOutput != null) textOutput.Data = text;
            DataChanged?.Invoke();
        }

        // IUI implementation
        System.Windows.UIElement IUI.GetWPFElement()
        {
            return textBox;
        }

        // IDataFlow<string> implementation
        string IDataFlow<string>.Data { set => textBox.Text = value; }

        // IDataFlowB<string> implementation
        public event DataChangedDelegate DataChanged;

        string IDataFlowB<string>.Data
        {
            get { Debug.WriteLine("Text sent!"); return text; }
        }

        // IEvent implementation
        void IEvent.Execute()
        {
            textBox.Clear();
        }

    }
}
