using System.Collections;
using System.Windows.Media;

namespace Node.Net.Deprecated.Controls.Internal
{
    class ImageUpdater
    {
        public ImageUpdater(System.Windows.Controls.Image image)
        {
            image.DataContextChanged += Image_DataContextChanged;
        }

        private void Image_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            UpdateImage(sender as System.Windows.Controls.Image);
        }

        public void UpdateImage(System.Windows.Controls.Image image)
        {
            image.Source = GetImageSource(image.DataContext);
        }

        private ImageSource GetImageSource(object value)
        {
            
            var imageSource = Factories.ImageSourceFactory.Default.Create<ImageSource>(value);
            if (imageSource == null)
            {
                imageSource = Internal.Extensions.IconExtensions.ToImageSource(Internal.Extensions.IconExtensions.GetIcon("Question"));
                //imageSource = Internal.Extensions.IconExtensions.GetIcon("Question").ToImageSource();
            }
            return imageSource;
        }
    }
}
