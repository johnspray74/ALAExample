using ProgrammingParadigms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DomainAbstractions
{
    /// <summary>
    /// A window with a list of WizardItems which display as radio buttons, and Buttons for Next, Cancel, and optionally Back.
    /// Each WizardItem has it's own boolean output. 
    /// When the Next key is pressed, the selected WizardItem's output goes true and emits an event. The Wizard hides but remains in an 'active' state which holds that output.
    /// The Back key, which only appears if it is connected somewhere, hides and deactivates the wizard.
    /// It typically connects to the left input of a previous wizard (which causes it to redisplay as it is usually active but hidden).
    /// The Cancel port is a bidirectional IEvent.When the Cancel key is pressed, or an event is received on this port, the wizard hides and deactivates, which releases any held output. 
    /// The IEvent for showing the window(if it's not active).
    ///
    /// See Macro for implementation. Wizard instance builds the other instances according to the macro diagram and information from the list of WizardItems. 
    /// ------------------------------------------------------------------------------------------------------------------
    /// Ports:
    /// IEvent open: opens the Wizard window
    /// List<IUIWizard> children: output list of UI elements contained in this Wizard (Note: wiring order determines order of position)
    /// IEvent backEventOutput: link to the Wizard which you would like the back button to go back to opening
    /// </summary>

    public class Wizard : IEvent // open
    {
        // properties 
        public string InstanceName = "Default";
        public string SecondTitle { set => titleTextBlock.Text = value; } 
        public bool ShowBackButton { set => InitializeBackButton(value); }
        public double Width { set => window.Width = value; }

        // ports
        private List<IUIWizard> children = new List<IUIWizard>();
        private IEvent backEventOutput;

        // private fields
        private Window window;
        private DockPanel backgroundPanel = new DockPanel();
        private TextBlock titleTextBlock = new TextBlock();
        private StackPanel contentPanel = new StackPanel();

        private StackPanel bottomPanel = new StackPanel();
        private System.Windows.Controls.Button backButton;
        private System.Windows.Controls.Button nextButton = new System.Windows.Controls.Button();
        private System.Windows.Controls.Button cancelButton = new System.Windows.Controls.Button();

        /// <summary>
        /// This is a specific popup window which organizes the WizardItem as it's sub-elements. 
        /// </summary>
        /// <param name="title">the text displayed at the top of the window</param>
        public Wizard(string title = "")
        {
            window = new Window()
            {
                Width = 555,
                Height = 400,
                Title = title,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                Topmost = false,
                Background = new SolidColorBrush(Color.FromRgb(245, 245, 245)),
                Content = GetInitializedBackgroundPanel()
            };

            // this event is added for handing the close action by user as a window instance cannot be open again after it is 
            // closed by click on the close button. However, we can reopen it after it's hided. So here we use hide() instead of close().
            window.Closing += (object sender, CancelEventArgs e) =>
            {
                e.Cancel = true;
                window.Hide();
            };
        }


        // --------------------------------------------------------------------------------------------------------
        // IEvent implementation
        void IEvent.Execute()
        {
            window.ShowDialog();
        }

        // button actions -------------------------------------------------------------------------------
        private void BackButtonClickedHandler(object sender, RoutedEventArgs e)
        {
            window.Hide();

            backEventOutput?.Execute();
        }

        private void NextButtonClickedHandler(object sender, RoutedEventArgs e)
        {
            window.Hide();

            children.Find(m => m.Checked).GenerateOutputEvent();
        }

        private void CancelButtonClickedHandler(object sender, RoutedEventArgs e) => window.Hide();


        // private methods ---------------------------------------------------------------------------------
        private DockPanel GetInitializedBackgroundPanel()
        {
            titleTextBlock.FontFamily = new FontFamily("Arial");
            titleTextBlock.FontSize = 20;
            titleTextBlock.FontWeight = FontWeights.Thin;
            titleTextBlock.Foreground = Brushes.Black;
            titleTextBlock.HorizontalAlignment = HorizontalAlignment.Center;
            titleTextBlock.TextAlignment = TextAlignment.Center;
            titleTextBlock.Margin = new Thickness(0, 15, 0, 10);
            DockPanel.SetDock(titleTextBlock, Dock.Top);
            backgroundPanel.Children.Add(titleTextBlock);

            bottomPanel.FlowDirection = FlowDirection.RightToLeft;
            bottomPanel.Orientation = Orientation.Horizontal;
            bottomPanel.Margin = new Thickness(0, 0, 0, 10);
            cancelButton.Content = "Cancel";
            cancelButton.Width = 80;
            cancelButton.Height = 28;
            cancelButton.Margin = new Thickness(10, 0, 8, 0);
            cancelButton.Click += CancelButtonClickedHandler;
            bottomPanel.Children.Add(cancelButton);
            nextButton.Content = "Next";
            nextButton.Width = 80;
            nextButton.Height = 28;
            nextButton.Click += NextButtonClickedHandler;
            bottomPanel.Children.Add(nextButton);
            DockPanel.SetDock(bottomPanel, Dock.Bottom);
            backgroundPanel.Children.Add(bottomPanel);

            contentPanel.Background = Brushes.White;
            contentPanel.Margin = new Thickness(10, 0, 10, 10);
            backgroundPanel.Children.Add(contentPanel);

            return backgroundPanel;
        }

        private void InitializeBackButton(bool show)
        {
            backButton = new System.Windows.Controls.Button();
            backButton.Content = "Back";
            backButton.Width = 80;
            backButton.Height = 28;
            backButton.Margin = new Thickness(8, 0, 0, 0);
            backButton.Click += BackButtonClickedHandler;
            bottomPanel.Children.Add(backButton);
        }

        private void PostWiringInitialize()
        {
            foreach (var c in children)  // fill in sub-elements
            {
                contentPanel.Children.Add(c.GetWPFElement());

                //UIElement element = c.GetWPFElement();
                ////contentPanel.Children.Add((element => c.GetWPFElement()): contentPanel.Children.Remove(c.GetWPFElement()), element );

                ////if(element)

                ////contentPanel.Children
                ////contentPanel.Children.Add(element { }
                ////}
                //if (contentPanel.Children.Contains(element))
                //{
                //    DependencyObject parent = VisualTreeHelper.GetParent(element);
                //    Wizard wizardParent = parent as IUI;
                //    contentPanel.Children.Remove(element);
                //}
                //contentPanel.Children.Add(element);

            }
        }
    }
}
