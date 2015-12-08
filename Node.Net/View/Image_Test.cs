using NUnit.Framework;
namespace Node.Net.View
{
    [TestFixture]
    class Image_Test
    {
        [TestCase,NUnit.Framework.Apartment(System.Threading.ApartmentState.STA),NUnit.Framework.Explicit]
        public void Image_Menger_Bitmap()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetAssembly(typeof(Image_Test));
            System.Windows.Media.Imaging.BitmapImage bitmapImage = new System.Windows.Media.Imaging.BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = assembly.GetManifestResourceStream("Node.Net.Resources.Menger.bmp");
            bitmapImage.EndInit();

            System.Windows.Controls.Image image = new System.Windows.Controls.Image();
            image.Source = bitmapImage;
            System.Windows.Window window = new System.Windows.Window();
            window.Content = image;
            window.Title = "Image_Menger_Bitmap";
            window.ShowDialog();
        }

        [TestCase,NUnit.Framework.Apartment(System.Threading.ApartmentState.STA),NUnit.Framework.Explicit]
        public void Image_Menger_Bitmap_Resize()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetAssembly(typeof(Image_Test));
            System.Windows.Media.Imaging.BitmapImage bitmapImage = new System.Windows.Media.Imaging.BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = assembly.GetManifestResourceStream("Node.Net.Resources.Menger.bmp");
            bitmapImage.EndInit();

            System.Windows.Controls.Image image = new System.Windows.Controls.Image();
            image.Source = bitmapImage;
            image.Height = 24;
            image.Width = 24;
            System.Windows.Window window = new System.Windows.Window();
            window.Content = image;
            window.Title = "Image_Menger_Bitmap_Resize";
            window.ShowDialog();
        }

        [TestCase,NUnit.Framework.Apartment(System.Threading.ApartmentState.STA),NUnit.Framework.Explicit]
        public void Image_Menger_Png()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetAssembly(typeof(Image_Test));
            System.Windows.Media.Imaging.BitmapImage bitmapImage = new System.Windows.Media.Imaging.BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = assembly.GetManifestResourceStream("Node.Net.Resources.Menger.png");
            bitmapImage.EndInit();

            System.Windows.Controls.Image image = new System.Windows.Controls.Image();
            image.Source = bitmapImage;
            System.Windows.Window window = new System.Windows.Window();
            window.Content = image;
            window.Title = "Image_Menger_Png";
            window.ShowDialog();
        }
        [TestCase,NUnit.Framework.Apartment(System.Threading.ApartmentState.STA),NUnit.Framework.Explicit]
        public void Image_Menger_Jpg()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetAssembly(typeof(Image_Test));
            System.Windows.Media.Imaging.BitmapImage bitmapImage = new System.Windows.Media.Imaging.BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = assembly.GetManifestResourceStream("Node.Net.Resources.Menger.jpg");
            bitmapImage.EndInit();

            System.Windows.Controls.Image image = new System.Windows.Controls.Image();
            image.Source = bitmapImage;
            System.Windows.Window window = new System.Windows.Window();
            window.Content = image;
            window.Title = "Image_Menger_Jpg";
            window.ShowDialog();
        }
    }
}
