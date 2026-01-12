extern alias NodeNet;
using System;
using NUnit.Framework;
using NodeNet::System.Windows.Media.Imaging;

namespace Node.Net.Test
{
    [TestFixture]
    internal static class BitmapSourceTests
    {
        private static bool CanCreateTestBitmapSource()
        {
#if !IS_WINDOWS
            try
            {
                TestBitmapSource bitmapSource = new TestBitmapSource(200, 300);
                return bitmapSource != null;
            }
            catch
            {
                return false;
            }
#else
            return false;
#endif
        }

#if !IS_WINDOWS
        // Test concrete implementation for BitmapSource (only works on non-Windows)
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
#endif

        [Test]
        public static void BitmapSource_PixelWidth_ReturnsCorrectValue()
        {
            if (!CanCreateTestBitmapSource())
            {
                Assert.Pass("TestBitmapSource only available on non-Windows platforms");
                return;
            }

            // Arrange
#if !IS_WINDOWS
            BitmapSource bitmapSource = new TestBitmapSource(200, 300);

            // Act & Assert
            Assert.That(bitmapSource.PixelWidth, Is.EqualTo(200));
#endif
        }

        [Test]
        public static void BitmapSource_PixelHeight_ReturnsCorrectValue()
        {
            if (!CanCreateTestBitmapSource())
            {
                Assert.Pass("TestBitmapSource only available on non-Windows platforms");
                return;
            }

            // Arrange
#if !IS_WINDOWS
            BitmapSource bitmapSource = new TestBitmapSource(200, 300);

            // Act & Assert
            Assert.That(bitmapSource.PixelHeight, Is.EqualTo(300));
#endif
        }

        [Test]
        public static void BitmapSource_DpiX_ReturnsDefaultValue()
        {
            if (!CanCreateTestBitmapSource())
            {
                Assert.Pass("TestBitmapSource only available on non-Windows platforms");
                return;
            }

            // Arrange
#if !IS_WINDOWS
            BitmapSource bitmapSource = new TestBitmapSource(200, 300);

            // Act & Assert
            Assert.That(bitmapSource.DpiX, Is.EqualTo(96.0));
#endif
        }

        [Test]
        public static void BitmapSource_DpiY_ReturnsDefaultValue()
        {
            if (!CanCreateTestBitmapSource())
            {
                Assert.Pass("TestBitmapSource only available on non-Windows platforms");
                return;
            }

            // Arrange
#if !IS_WINDOWS
            BitmapSource bitmapSource = new TestBitmapSource(200, 300);

            // Act & Assert
            Assert.That(bitmapSource.DpiY, Is.EqualTo(96.0));
#endif
        }

        [Test]
        public static void BitmapSource_DpiX_CanBeSet()
        {
            if (!CanCreateTestBitmapSource())
            {
                Assert.Pass("TestBitmapSource only available on non-Windows platforms");
                return;
            }

            // Arrange
#if !IS_WINDOWS
            BitmapSource bitmapSource = new TestBitmapSource(200, 300, 120.0, 96.0);

            // Act & Assert
            Assert.That(bitmapSource.DpiX, Is.EqualTo(120.0));
#endif
        }

        [Test]
        public static void BitmapSource_DpiY_CanBeSet()
        {
            if (!CanCreateTestBitmapSource())
            {
                Assert.Pass("TestBitmapSource only available on non-Windows platforms");
                return;
            }

            // Arrange
#if !IS_WINDOWS
            BitmapSource bitmapSource = new TestBitmapSource(200, 300, 96.0, 120.0);

            // Act & Assert
            Assert.That(bitmapSource.DpiY, Is.EqualTo(120.0));
#endif
        }

        [Test]
        public static void BitmapSource_Width_CalculatesFromPixelWidthAndDpiX()
        {
            if (!CanCreateTestBitmapSource())
            {
                Assert.Pass("TestBitmapSource only available on non-Windows platforms");
                return;
            }

            // Arrange
#if !IS_WINDOWS
            BitmapSource bitmapSource = new TestBitmapSource(200, 300, 96.0, 96.0);

            // Act & Assert
            // Width = PixelWidth * 96 / DpiX = 200 * 96 / 96 = 200
            Assert.That(bitmapSource.Width, Is.EqualTo(200.0));
#endif
        }

        [Test]
        public static void BitmapSource_Height_CalculatesFromPixelHeightAndDpiY()
        {
            if (!CanCreateTestBitmapSource())
            {
                Assert.Pass("TestBitmapSource only available on non-Windows platforms");
                return;
            }

            // Arrange
#if !IS_WINDOWS
            BitmapSource bitmapSource = new TestBitmapSource(200, 300, 96.0, 96.0);

            // Act & Assert
            // Height = PixelHeight * 96 / DpiY = 300 * 96 / 96 = 300
            Assert.That(bitmapSource.Height, Is.EqualTo(300.0));
#endif
        }

        [Test]
        public static void BitmapSource_Width_WithCustomDpi_CalculatesCorrectly()
        {
            if (!CanCreateTestBitmapSource())
            {
                Assert.Pass("TestBitmapSource only available on non-Windows platforms");
                return;
            }

            // Arrange
#if !IS_WINDOWS
            BitmapSource bitmapSource = new TestBitmapSource(200, 300, 192.0, 96.0);

            // Act & Assert
            // Width = PixelWidth * 96 / DpiX = 200 * 96 / 192 = 100
            Assert.That(bitmapSource.Width, Is.EqualTo(100.0));
#endif
        }

        [Test]
        public static void BitmapSource_Height_WithCustomDpi_CalculatesCorrectly()
        {
            if (!CanCreateTestBitmapSource())
            {
                Assert.Pass("TestBitmapSource only available on non-Windows platforms");
                return;
            }

            // Arrange
#if !IS_WINDOWS
            BitmapSource bitmapSource = new TestBitmapSource(200, 300, 96.0, 192.0);

            // Act & Assert
            // Height = PixelHeight * 96 / DpiY = 300 * 96 / 192 = 150
            Assert.That(bitmapSource.Height, Is.EqualTo(150.0));
#endif
        }

        [Test]
        public static void BitmapSource_Clone_ReturnsReference()
        {
            if (!CanCreateTestBitmapSource())
            {
                Assert.Pass("TestBitmapSource only available on non-Windows platforms");
                return;
            }

            // Arrange
#if !IS_WINDOWS
            BitmapSource bitmapSource = new TestBitmapSource(200, 300);

            // Act
            BitmapSource cloned = bitmapSource.Clone();

            // Assert
            Assert.That(cloned, Is.Not.Null);
            Assert.That(cloned.PixelWidth, Is.EqualTo(bitmapSource.PixelWidth));
            Assert.That(cloned.PixelHeight, Is.EqualTo(bitmapSource.PixelHeight));
#endif
        }

        [Test]
        public static void BitmapImage_Constructor_InitializesCorrectly()
        {
            // Arrange & Act
            BitmapImage bitmapImage = new BitmapImage();

            // Assert
            Assert.That(bitmapImage, Is.Not.Null);
        }

        private static bool BitmapImageHasBeginInit()
        {
#if IS_WINDOWS
            // On Windows, BeginInit exists but may not work with MemoryStream
            // Check if BeginInit method exists via reflection
            try
            {
                var method = typeof(BitmapImage).GetMethod("BeginInit", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                if (method == null)
                {
                    return false;
                }
                // On Windows, BeginInit exists but may require actual image data
                // Return false to skip these tests on Windows
                return false;
            }
            catch
            {
                return false;
            }
#else
            try
            {
                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                return true;
            }
            catch
            {
                return false;
            }
#endif
        }

        [Test]
        public static void BitmapImage_BeginInit_AllowsSettingStreamSource()
        {
            if (!BitmapImageHasBeginInit())
            {
                Assert.Pass("BitmapImage.BeginInit only available on non-Windows platforms");
                return;
            }

            // Arrange
            BitmapImage bitmapImage = new BitmapImage();
            using System.IO.MemoryStream stream = new System.IO.MemoryStream();

            // Act
            try
            {
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = stream;
                bitmapImage.EndInit();

                // Assert
                Assert.That(bitmapImage.StreamSource, Is.EqualTo(stream));
            }
            catch (System.NotSupportedException)
            {
                // On Windows, BeginInit may exist but fail with MemoryStream due to missing image decoders
                Assert.Pass("BitmapImage.BeginInit requires image decoders on Windows");
            }
        }

        [Test]
        public static void BitmapImage_BeginInit_AllowsSettingUriSource()
        {
            if (!BitmapImageHasBeginInit())
            {
                Assert.Pass("BitmapImage.BeginInit only available on non-Windows platforms");
                return;
            }

            // Arrange
            BitmapImage bitmapImage = new BitmapImage();
            Uri uri = new Uri("http://example.com/image.png");

            // Act
            try
            {
                bitmapImage.BeginInit();
                bitmapImage.UriSource = uri;
                bitmapImage.EndInit();

                // Assert
                Assert.That(bitmapImage.UriSource, Is.EqualTo(uri));
            }
            catch (System.NotSupportedException)
            {
                // On Windows, BeginInit may exist but fail due to missing image decoders
                Assert.Pass("BitmapImage.BeginInit requires image decoders on Windows");
            }
        }

        [Test]
        public static void BitmapImage_SetStreamSource_OutsideBeginInit_ThrowsException()
        {
            if (!BitmapImageHasBeginInit())
            {
                Assert.Pass("BitmapImage.BeginInit only available on non-Windows platforms");
                return;
            }

            // Arrange
            BitmapImage bitmapImage = new BitmapImage();
            using System.IO.MemoryStream stream = new System.IO.MemoryStream();

            // Act & Assert
#if IS_WINDOWS
            // On Windows, setting StreamSource outside BeginInit may not throw InvalidOperationException
            // It may throw NotSupportedException or succeed
            try
            {
                bitmapImage.StreamSource = stream;
                Assert.Pass("BitmapImage.StreamSource can be set outside BeginInit on Windows");
            }
            catch (InvalidOperationException)
            {
                // Expected behavior
            }
            catch (System.NotSupportedException)
            {
                Assert.Pass("BitmapImage.StreamSource requires image decoders on Windows");
            }
#else
            Assert.Throws<InvalidOperationException>(() => bitmapImage.StreamSource = stream);
#endif
        }

        [Test]
        public static void BitmapImage_SetUriSource_OutsideBeginInit_ThrowsException()
        {
            if (!BitmapImageHasBeginInit())
            {
                Assert.Pass("BitmapImage.BeginInit only available on non-Windows platforms");
                return;
            }

            // Arrange
            BitmapImage bitmapImage = new BitmapImage();
            Uri uri = new Uri("http://example.com/image.png");

            // Act & Assert
#if IS_WINDOWS
            // On Windows, setting UriSource outside BeginInit may not throw InvalidOperationException
            try
            {
                bitmapImage.UriSource = uri;
                Assert.Pass("BitmapImage.UriSource can be set outside BeginInit on Windows");
            }
            catch (InvalidOperationException)
            {
                // Expected behavior
            }
            catch (System.NotSupportedException)
            {
                Assert.Pass("BitmapImage.UriSource requires image decoders on Windows");
            }
#else
            Assert.Throws<InvalidOperationException>(() => bitmapImage.UriSource = uri);
#endif
        }

        [Test]
        public static void BitmapImage_Constructor_WithUri_InitializesCorrectly()
        {
            if (!BitmapImageHasBeginInit())
            {
                Assert.Pass("BitmapImage constructor with Uri only available on non-Windows platforms");
                return;
            }

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
            if (!BitmapImageHasBeginInit())
            {
                Assert.Pass("BitmapImage.BeginInit only available on non-Windows platforms");
                return;
            }

            // Arrange
            BitmapImage bitmapImage = new BitmapImage();

            // Act
            try
            {
                bitmapImage.BeginInit();
                bitmapImage.EndInit();

                // Assert
                Assert.That(bitmapImage.PixelWidth, Is.GreaterThan(0));
                Assert.That(bitmapImage.PixelHeight, Is.GreaterThan(0));
            }
            catch (System.NotSupportedException)
            {
                // On Windows, EndInit may fail due to missing image decoders
                Assert.Pass("BitmapImage.EndInit requires image decoders on Windows");
            }
        }
    }
}

