using System;
using System.Threading.Tasks;

namespace Node.Net.Test
{
    internal class ImageSourceTests
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
        public async Task ImageSource_Width_ReturnsCorrectValue()
        {
            if (!CanCreateTestImageSource())
            {
                // TUnit doesn't have Assert.Pass - just return early
                return;
            }

            // Arrange
#if !IS_WINDOWS
            ImageSource imageSource = new TestImageSource(100.0, 200.0);

            // Act & Assert
            await Assert.That(imageSource.Width).IsEqualTo(100.0);
#endif
        }

        [Test]
        public async Task ImageSource_Height_ReturnsCorrectValue()
        {
            if (!CanCreateTestImageSource())
            {
                // TUnit doesn't have Assert.Pass - just return early
                return;
            }

            // Arrange
#if !IS_WINDOWS
            ImageSource imageSource = new TestImageSource(100.0, 200.0);

            // Act & Assert
            await Assert.That(imageSource.Height).IsEqualTo(200.0);
#endif
        }

        [Test]
        public async Task ImageSource_ToString_ReturnsFormattedString()
        {
            if (!CanCreateTestImageSource())
            {
                // TUnit doesn't have Assert.Pass - just return early
                return;
            }

            // Arrange
#if !IS_WINDOWS
            ImageSource imageSource = new TestImageSource(100.0, 200.0);

            // Act
            string result = imageSource.ToString();

            // Assert
            await Assert.That(result.Contains("TestImageSource")).IsTrue();
            await Assert.That(result.Contains("Width=100")).IsTrue();
            await Assert.That(result.Contains("Height=200")).IsTrue();
#endif
        }

        [Test]
        public async Task ImageSource_Metadata_ReturnsNullByDefault()
        {
            if (!CanCreateTestImageSource())
            {
                // TUnit doesn't have Assert.Pass - just return early
                return;
            }

            // Arrange
#if !IS_WINDOWS
            ImageSource imageSource = new TestImageSource(100.0, 200.0);

            // Act & Assert
            await Assert.That(imageSource.Metadata).IsNull();
#endif
        }
    }
}

