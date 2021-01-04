using ProgrammingParadigms;
using System;
using System.Reflection;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using WpfAnimatedGif;

namespace DomainAbstractions
{
    /// <summary>
    /// This abstraction is used for loading and displaying resource files, usually for image files.
    /// </summary>
    public class Picture : IUI
    {
        // properties
        public string InstanceName = "Default";
        public double Width { set => image.Width = value; }
        public double Height { set => image.Height = value; }

        // private fields
        private Image image;

        /// <summary>
        /// A specific IUI abstraction which returns the UI element of an Image.
        /// </summary>
        /// <param name="imageName">the file name of the image (without path)</param>
        public Picture(string imageName)
        {
            image = new Image();
            image.Source = new BitmapImage(new Uri(@"pack://application:,,,/" + Assembly.GetExecutingAssembly().GetName().Name + @";component/Application/" + string.Format("Resources/{0}", imageName), UriKind.Absolute));
            if (imageName.EndsWith(".gif"))
            {
                Image animatedImage = new Image();
                ImageBehavior.SetAnimatedSource(animatedImage, image.Source);
                image = animatedImage;
            }
        }

        // IUI implementation -------------------------------------------------------------
        UIElement IUI.GetWPFElement()
        {
            return image;
        }
    }
}
