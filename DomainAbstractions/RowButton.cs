using ProgrammingParadigms;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace DomainAbstractions
{

    /// <summary>
    /// An IUI button that looks like a row of a grid. Displays itself in a grid cell and occupies the full space of the cell
    /// Can be wired to any kind of WPF element.
    /// ------------------------------------------------------------------------------------------------------------------
    /// Ports:
    /// 1. IUI inputIUI: input IUI to get the WPF element
    /// 2. IDataFlow<string> textContent: for the content it displays
    /// 3. IDataFlow<bool> visible: to determine visibility
    /// 4. IEvent eventButtonClicked: event ouput for when the RowButton is clicked
    /// </summary>
    public class RowButton : IUI, IDataFlow<string>, IDataFlow<bool>, IEvent // inputIUI, textContent, visible
    {
        // properties
        public string InstanceName = "Default";
        public Thickness Margin { set => backgroundBorder.Margin = value;  }
        public double Height { set => textBackground.Height = value;  }

        public Visibility Visible { set => backgroundBorder.Visibility = value; }

        // outputs
        private IEvent eventButtonClicked;

        // private fields
        private Border backgroundBorder = new Border();
        private Border textBackground = new Border();
        private TextBlock contentTextBlock = new TextBlock();

        /// <summary>
        /// A specific IUI button which displays itself in a grid cell and occupies the full space of the cell.
        /// </summary>
        /// <param name="title">the content of the button</param>
        public RowButton(string title = null)
        {
            contentTextBlock.Text = title;
            contentTextBlock.FontSize = 15;
            contentTextBlock.Foreground = Brushes.White;
            contentTextBlock.VerticalAlignment = VerticalAlignment.Center;
            contentTextBlock.IsEnabled = string.IsNullOrEmpty(title) ? false : true;
            contentTextBlock.MouseLeftButtonDown += (object sender, MouseButtonEventArgs e) => {
                contentTextBlock.Foreground = Brushes.White;
                textBackground.Background = Brushes.DodgerBlue;
                eventButtonClicked?.Execute();
            };

            textBackground.Margin = new Thickness(2);
            textBackground.Child = contentTextBlock;
            textBackground.VerticalAlignment = VerticalAlignment.Center;

            // backgroundBorder.Background = Brushes.White;
            backgroundBorder.Background = Brushes.LightGray;
            backgroundBorder.Child = textBackground;

        }

        // IUI implementation -----------------------------------------------------------
        UIElement IUI.GetWPFElement()
        {            
            return backgroundBorder;
        }

        // IDataFlow<string> implementation -----------------------------------------------------------
        string IDataFlow<string>.Data {
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    textBackground.Background = Brushes.White;
                    textBackground.Height = 50;
                    contentTextBlock.Foreground = Brushes.DodgerBlue;
                    contentTextBlock.Text = value;
                    contentTextBlock.IsEnabled = false;
                }
                else
                {
                    textBackground.Background = Brushes.White;
                    textBackground.Height = 30;
                    contentTextBlock.Foreground = Brushes.Black;
                    contentTextBlock.Text = value;
                    contentTextBlock.IsEnabled = true;
                }
            }
        }

        // IDataFlow<bool> implementation -----------------------------------------------
        bool IDataFlow<bool>.Data { set => backgroundBorder.Visibility = value ? Visibility.Visible : Visibility.Collapsed; }

        // IEvent implementation

        void IEvent.Execute()
        {
            // reset roe button UI
            contentTextBlock.Foreground = Brushes.Black;
            textBackground.Background = Brushes.White;
        }
    }
}
