using NUnit.Framework;
namespace Node.Net.View
{
    [TestFixture]
    class Image_Test
    {
        [TestCase,NUnit.Framework.Apartment(System.Threading.ApartmentState.STA),NUnit.Framework.Explicit]
        public void Image_Menger_Bitmap()
        {
            var assembly = System.Reflection.Assembly.GetAssembly(typeof(Image_Test));
            var bitmapImage = new System.Windows.Media.Imaging.BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = assembly.GetManifestResourceStream("Node.Net.Resources.Menger.bmp");
            bitmapImage.EndInit();

            var image = new System.Windows.Controls.Image
            {
                Source = bitmapImage
            };
            var window = new System.Windows.Window
            {
                Content = image,
                Title = nameof(Image_Menger_Bitmap)
            };
            window.ShowDialog();
        }

        [TestCase,NUnit.Framework.Apartment(System.Threading.ApartmentState.STA),NUnit.Framework.Explicit]
        public void Image_Menger_Bitmap_Resize()
        {
            var assembly = System.Reflection.Assembly.GetAssembly(typeof(Image_Test));
            var bitmapImage = new System.Windows.Media.Imaging.BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = assembly.GetManifestResourceStream("Node.Net.Resources.Menger.bmp");
            bitmapImage.EndInit();

            var image = new System.Windows.Controls.Image
            {
                Source = bitmapImage,
                Height = 24,
                Width = 24
            };
            var window = new System.Windows.Window
            {
                Content = image,
                Title = nameof(Image_Menger_Bitmap_Resize)
            };
            window.ShowDialog();
        }

        [TestCase,NUnit.Framework.Apartment(System.Threading.ApartmentState.STA),NUnit.Framework.Explicit]
        public void Image_Menger_Png()
        {
            var assembly = System.Reflection.Assembly.GetAssembly(typeof(Image_Test));
            var bitmapImage = new System.Windows.Media.Imaging.BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = assembly.GetManifestResourceStream("Node.Net.Resources.Menger.png");
            bitmapImage.EndInit();

            var image = new System.Windows.Controls.Image
            {
                Source = bitmapImage
            };
            var window = new System.Windows.Window
            {
                Content = image,
                Title = nameof(Image_Menger_Png)
            };
            window.ShowDialog();
        }
        [TestCase,NUnit.Framework.Apartment(System.Threading.ApartmentState.STA),NUnit.Framework.Explicit]
        public void Image_Menger_Jpg()
        {
            var assembly = System.Reflection.Assembly.GetAssembly(typeof(Image_Test));
            var bitmapImage = new System.Windows.Media.Imaging.BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = assembly.GetManifestResourceStream("Node.Net.Resources.Menger.jpg");
            bitmapImage.EndInit();

            var image = new System.Windows.Controls.Image
            {
                Source = bitmapImage
            };
            var window = new System.Windows.Window
            {
                Content = image,
                Title = nameof(Image_Menger_Jpg)
            };
            window.ShowDialog();
        }
    }
}
