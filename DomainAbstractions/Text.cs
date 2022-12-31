using ProgrammingParadigms;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DomainAbstractions
{
    /// <summary>
    /// A UI element, just like a label with a text string on it. 
    /// ---------------------------------------------------------
    /// Ports: 
    /// 1. IUI iui: Returns the IUI element.
    /// 2. IDataFlow<string> text: Sets the value of the text field.
    /// 3. IDataFlow<bool> visible: Sets the visibility of the text. 
    /// </summary>
    public class Text : IUI, IDataFlow<string>, IDataFlow<bool> // iui, text, visible
    {
        // properties
        public string InstanceName = "Default";
        public Brush Color = Brushes.Black;
        public Brush Background;
        public double HeightRatio;
        public Thickness Margin;
        public Thickness Padding;
        public bool ShowBorder = false;
        public HorizontalAlignment HorizAlignment
        {
            set => textBlock.HorizontalAlignment = value;
        }

        public Visibility Visibility
        {
            set
            {
                textBlock.Visibility = value;
            }
        }

        // properties for customizing UI
        public FontWeight FontWeight { set => textBlock.FontWeight = value; }
        public FontStyle FontStyle { set => textBlock.FontStyle = value; }
        public double FontSize { set => textBlock.FontSize = value; }
        public string Content { set => textBlock.Text = value; }

        // private fields
        private UIElement wpfElement;
        private TextBlock textBlock = new TextBlock();

        /// <summary>
        /// An IUI abstraction which disaplys a giving text.
        /// </summary>
        /// <param name="text">text displayed</param>
        /// <param name="visible">control the visibility of the text</param>
        /// <param name="wrap">whether the text should wrap</param>
        public Text(string text = null, bool visible = true, bool wrap = true)
        {
            textBlock.Text = text;
            textBlock.VerticalAlignment = VerticalAlignment.Center;
            //textBlock.FontFamily = new FontFamily("Arial");
            textBlock.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
            textBlock.TextWrapping = wrap ? TextWrapping.Wrap : TextWrapping.NoWrap;
        }

        // IUI implementation ---------------------------------------------------------------
        UIElement IUI.GetWPFElement()
        {
            if (HeightRatio != 0)
            {
                textBlock.LayoutTransform = new ScaleTransform() { ScaleY = HeightRatio };
            }

            if (ShowBorder)
            {
                var border = new Border();
                border.Margin = Margin;
                border.Child = textBlock;
                border.BorderThickness = new Thickness(1);
                border.BorderBrush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(180, 180, 180));
                textBlock.Padding = Padding;
                wpfElement = border;
            }
            else
            {
                textBlock.Margin = Margin;
                wpfElement = textBlock;
            }

            textBlock.Foreground = Color;
            if (Background != null) textBlock.Background = Background;

            return wpfElement;
        }

        // IDataFlow<string> implementation ---------------------------------------------------------------
        void IDataFlow<string>.Push(string data) { textBlock.Text = data; }

        // IDataFlow<bool> implementation -----------------------------------------------------------------
        void IDataFlow<bool>.Push(bool data) { wpfElement.Visibility = data ? Visibility.Visible : Visibility.Collapsed; }
    }
}
