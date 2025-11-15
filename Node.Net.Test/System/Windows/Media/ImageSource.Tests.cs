using System;
using NUnit.Framework;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Node.Net.Test
{
    [TestFixture]
    internal static class ImageSourceTests
    {
#if !IS_WINDOWS
        // Test concrete implementation for ImageSource
        private class TestImageSource : ImageSource
        {
            private double _width;
            private double _height;

            public TestImageSource(double width, double height)
            {
                _width = width;
                _height = height;
            }

            public override double Width => _width;
            public override double Height => _height;
        }

        [Test]
        public static void ImageSource_Width_ReturnsCorrectValue()
        {
            // Arrange
            ImageSource imageSource = new TestImageSource(100.0, 200.0);

            // Act & Assert
            Assert.That(imageSource.Width, Is.EqualTo(100.0));
        }

        [Test]
        public static void ImageSource_Height_ReturnsCorrectValue()
        {
            // Arrange
            ImageSource imageSource = new TestImageSource(100.0, 200.0);

            // Act & Assert
            Assert.That(imageSource.Height, Is.EqualTo(200.0));
        }

        [Test]
        public static void ImageSource_ToString_ReturnsFormattedString()
        {
            // Arrange
            ImageSource imageSource = new TestImageSource(100.0, 200.0);

            // Act
            string result = imageSource.ToString();

            // Assert
            Assert.That(result, Does.Contain("TestImageSource"));
            Assert.That(result, Does.Contain("Width=100"));
            Assert.That(result, Does.Contain("Height=200"));
        }

        [Test]
        public static void ImageSource_Metadata_ReturnsNullByDefault()
        {
            // Arrange
            ImageSource imageSource = new TestImageSource(100.0, 200.0);

            // Act & Assert
            Assert.That(imageSource.Metadata, Is.Null);
        }
#endif
    }
}

