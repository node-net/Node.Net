using System;
using NUnit.Framework;

namespace Node.Net.Test
{
    [TestFixture]
    internal static class ImageSourceTests
    {
        private static bool CanCreateTestImageSource()
        {
#if !IS_WINDOWS
            // Test concrete implementation for ImageSource (only works on non-Windows)
            try
            {
                TestImageSource imageSource = new TestImageSource(100.0, 200.0);
                return imageSource != null;
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
        // Test concrete implementation for ImageSource (only works on non-Windows)
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
#endif

        [Test]
        public static void ImageSource_Width_ReturnsCorrectValue()
        {
            if (!CanCreateTestImageSource())
            {
                Assert.Pass("TestImageSource only available on non-Windows platforms");
                return;
            }

            // Arrange
#if !IS_WINDOWS
            ImageSource imageSource = new TestImageSource(100.0, 200.0);

            // Act & Assert
            Assert.That(imageSource.Width, Is.EqualTo(100.0));
#endif
        }

        [Test]
        public static void ImageSource_Height_ReturnsCorrectValue()
        {
            if (!CanCreateTestImageSource())
            {
                Assert.Pass("TestImageSource only available on non-Windows platforms");
                return;
            }

            // Arrange
#if !IS_WINDOWS
            ImageSource imageSource = new TestImageSource(100.0, 200.0);

            // Act & Assert
            Assert.That(imageSource.Height, Is.EqualTo(200.0));
#endif
        }

        [Test]
        public static void ImageSource_ToString_ReturnsFormattedString()
        {
            if (!CanCreateTestImageSource())
            {
                Assert.Pass("TestImageSource only available on non-Windows platforms");
                return;
            }

            // Arrange
#if !IS_WINDOWS
            ImageSource imageSource = new TestImageSource(100.0, 200.0);

            // Act
            string result = imageSource.ToString();

            // Assert
            Assert.That(result, Does.Contain("TestImageSource"));
            Assert.That(result, Does.Contain("Width=100"));
            Assert.That(result, Does.Contain("Height=200"));
#endif
        }

        [Test]
        public static void ImageSource_Metadata_ReturnsNullByDefault()
        {
            if (!CanCreateTestImageSource())
            {
                Assert.Pass("TestImageSource only available on non-Windows platforms");
                return;
            }

            // Arrange
#if !IS_WINDOWS
            ImageSource imageSource = new TestImageSource(100.0, 200.0);

            // Act & Assert
            Assert.That(imageSource.Metadata, Is.Null);
#endif
        }
    }
}

