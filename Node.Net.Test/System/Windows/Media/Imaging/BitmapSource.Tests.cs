using System;
using NUnit.Framework;
using System.Windows.Media.Imaging;

namespace Node.Net.Test
{
    [TestFixture]
    internal static class BitmapSourceTests
    {
#if !IS_WINDOWS
        // Test concrete implementation for BitmapSource
        private class TestBitmapSource : BitmapSource
        {
            public TestBitmapSource(int pixelWidth, int pixelHeight, double dpiX = 96.0, double dpiY = 96.0)
            {
                PixelWidth = pixelWidth;
                PixelHeight = pixelHeight;
                DpiX = dpiX;
                DpiY = dpiY;
            }
        }

        [Test]
        public static void BitmapSource_PixelWidth_ReturnsCorrectValue()
        {
            // Arrange
            BitmapSource bitmapSource = new TestBitmapSource(200, 300);

            // Act & Assert
            Assert.That(bitmapSource.PixelWidth, Is.EqualTo(200));
        }

        [Test]
        public static void BitmapSource_PixelHeight_ReturnsCorrectValue()
        {
            // Arrange
            BitmapSource bitmapSource = new TestBitmapSource(200, 300);

            // Act & Assert
            Assert.That(bitmapSource.PixelHeight, Is.EqualTo(300));
        }

        [Test]
        public static void BitmapSource_DpiX_ReturnsDefaultValue()
        {
            // Arrange
            BitmapSource bitmapSource = new TestBitmapSource(200, 300);

            // Act & Assert
            Assert.That(bitmapSource.DpiX, Is.EqualTo(96.0));
        }

        [Test]
        public static void BitmapSource_DpiY_ReturnsDefaultValue()
        {
            // Arrange
            BitmapSource bitmapSource = new TestBitmapSource(200, 300);

            // Act & Assert
            Assert.That(bitmapSource.DpiY, Is.EqualTo(96.0));
        }

        [Test]
        public static void BitmapSource_DpiX_CanBeSet()
        {
            // Arrange
            BitmapSource bitmapSource = new TestBitmapSource(200, 300, 120.0, 96.0);

            // Act & Assert
            Assert.That(bitmapSource.DpiX, Is.EqualTo(120.0));
        }

        [Test]
        public static void BitmapSource_DpiY_CanBeSet()
        {
            // Arrange
            BitmapSource bitmapSource = new TestBitmapSource(200, 300, 96.0, 120.0);

            // Act & Assert
            Assert.That(bitmapSource.DpiY, Is.EqualTo(120.0));
        }

        [Test]
        public static void BitmapSource_Width_CalculatesFromPixelWidthAndDpiX()
        {
            // Arrange
            BitmapSource bitmapSource = new TestBitmapSource(200, 300, 96.0, 96.0);

            // Act & Assert
            // Width = PixelWidth * 96 / DpiX = 200 * 96 / 96 = 200
            Assert.That(bitmapSource.Width, Is.EqualTo(200.0));
        }

        [Test]
        public static void BitmapSource_Height_CalculatesFromPixelHeightAndDpiY()
        {
            // Arrange
            BitmapSource bitmapSource = new TestBitmapSource(200, 300, 96.0, 96.0);

            // Act & Assert
            // Height = PixelHeight * 96 / DpiY = 300 * 96 / 96 = 300
            Assert.That(bitmapSource.Height, Is.EqualTo(300.0));
        }

        [Test]
        public static void BitmapSource_Width_WithCustomDpi_CalculatesCorrectly()
        {
            // Arrange
            BitmapSource bitmapSource = new TestBitmapSource(200, 300, 192.0, 96.0);

            // Act & Assert
            // Width = PixelWidth * 96 / DpiX = 200 * 96 / 192 = 100
            Assert.That(bitmapSource.Width, Is.EqualTo(100.0));
        }

        [Test]
        public static void BitmapSource_Height_WithCustomDpi_CalculatesCorrectly()
        {
            // Arrange
            BitmapSource bitmapSource = new TestBitmapSource(200, 300, 96.0, 192.0);

            // Act & Assert
            // Height = PixelHeight * 96 / DpiY = 300 * 96 / 192 = 150
            Assert.That(bitmapSource.Height, Is.EqualTo(150.0));
        }

        [Test]
        public static void BitmapSource_Clone_ReturnsReference()
        {
            // Arrange
            BitmapSource bitmapSource = new TestBitmapSource(200, 300);

            // Act
            BitmapSource cloned = bitmapSource.Clone();

            // Assert
            Assert.That(cloned, Is.Not.Null);
            Assert.That(cloned.PixelWidth, Is.EqualTo(bitmapSource.PixelWidth));
            Assert.That(cloned.PixelHeight, Is.EqualTo(bitmapSource.PixelHeight));
        }
#endif

        [Test]
        public static void BitmapImage_Constructor_InitializesCorrectly()
        {
            // Arrange & Act
#if IS_WINDOWS
            BitmapImage bitmapImage = new BitmapImage();
#else
            BitmapImage bitmapImage = new BitmapImage();
#endif

            // Assert
            Assert.That(bitmapImage, Is.Not.Null);
        }

#if !IS_WINDOWS
        [Test]
        public static void BitmapImage_BeginInit_AllowsSettingStreamSource()
        {
            // Arrange
            BitmapImage bitmapImage = new BitmapImage();
            using System.IO.MemoryStream stream = new System.IO.MemoryStream();

            // Act
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = stream;
            bitmapImage.EndInit();

            // Assert
            Assert.That(bitmapImage.StreamSource, Is.EqualTo(stream));
        }

        [Test]
        public static void BitmapImage_BeginInit_AllowsSettingUriSource()
        {
            // Arrange
            BitmapImage bitmapImage = new BitmapImage();
            Uri uri = new Uri("http://example.com/image.png");

            // Act
            bitmapImage.BeginInit();
            bitmapImage.UriSource = uri;
            bitmapImage.EndInit();

            // Assert
            Assert.That(bitmapImage.UriSource, Is.EqualTo(uri));
        }

        [Test]
        public static void BitmapImage_SetStreamSource_OutsideBeginInit_ThrowsException()
        {
            // Arrange
            BitmapImage bitmapImage = new BitmapImage();
            using System.IO.MemoryStream stream = new System.IO.MemoryStream();

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => bitmapImage.StreamSource = stream);
        }

        [Test]
        public static void BitmapImage_SetUriSource_OutsideBeginInit_ThrowsException()
        {
            // Arrange
            BitmapImage bitmapImage = new BitmapImage();
            Uri uri = new Uri("http://example.com/image.png");

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => bitmapImage.UriSource = uri);
        }

        [Test]
        public static void BitmapImage_Constructor_WithUri_InitializesCorrectly()
        {
            // Arrange
            Uri uri = new Uri("http://example.com/image.png");

            // Act
            BitmapImage bitmapImage = new BitmapImage(uri);

            // Assert
            Assert.That(bitmapImage.UriSource, Is.EqualTo(uri));
        }

        [Test]
        public static void BitmapImage_EndInit_SetsDefaultDimensions()
        {
            // Arrange
            BitmapImage bitmapImage = new BitmapImage();

            // Act
            bitmapImage.BeginInit();
            bitmapImage.EndInit();

            // Assert
            Assert.That(bitmapImage.PixelWidth, Is.GreaterThan(0));
            Assert.That(bitmapImage.PixelHeight, Is.GreaterThan(0));
        }
#endif
    }
}

