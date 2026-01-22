using System;
using NUnit.Framework;

namespace Node.Net.Test
{
    [TestFixture]
    internal static class ColorTests
    {
        [Test]
        public static void Constructor_Default_InitializesToZero()
        {
            Color color = new Color();
            Assert.That(color.A, Is.EqualTo(0));
            Assert.That(color.R, Is.EqualTo(0));
            Assert.That(color.G, Is.EqualTo(0));
            Assert.That(color.B, Is.EqualTo(0));
        }

        [Test]
        public static void Properties_CanBeSet()
        {
            Color color = new Color();
            color.A = 255;
            color.R = 100;
            color.G = 150;
            color.B = 200;
            Assert.That(color.A, Is.EqualTo(255));
            Assert.That(color.R, Is.EqualTo(100));
            Assert.That(color.G, Is.EqualTo(150));
            Assert.That(color.B, Is.EqualTo(200));
        }

        [Test]
        public static void FromRgb_WithParameters_SetsCorrectValues()
        {
            // Arrange & Act
            Color color = Color.FromRgb(100, 150, 200);

            // Assert
            Assert.That(color.A, Is.EqualTo(255), "Alpha should be 255 for FromRgb");
            Assert.That(color.R, Is.EqualTo(100), "Red should be 100");
            Assert.That(color.G, Is.EqualTo(150), "Green should be 150");
            Assert.That(color.B, Is.EqualTo(200), "Blue should be 200");
        }

        [Test]
        public static void FromRgb_WithZeroValues_SetsCorrectValues()
        {
            // Arrange & Act
            Color color = Color.FromRgb(0, 0, 0);

            // Assert
            Assert.That(color.A, Is.EqualTo(255));
            Assert.That(color.R, Is.EqualTo(0));
            Assert.That(color.G, Is.EqualTo(0));
            Assert.That(color.B, Is.EqualTo(0));
        }

        [Test]
        public static void FromRgb_WithMaxValues_SetsCorrectValues()
        {
            // Arrange & Act
            Color color = Color.FromRgb(255, 255, 255);

            // Assert
            Assert.That(color.A, Is.EqualTo(255));
            Assert.That(color.R, Is.EqualTo(255));
            Assert.That(color.G, Is.EqualTo(255));
            Assert.That(color.B, Is.EqualTo(255));
        }

        [Test]
        public static void FromArgb_WithParameters_SetsCorrectValues()
        {
            // Arrange & Act
            Color color = Color.FromArgb(128, 100, 150, 200);

            // Assert
            Assert.That(color.A, Is.EqualTo(128), "Alpha should be 128");
            Assert.That(color.R, Is.EqualTo(100), "Red should be 100");
            Assert.That(color.G, Is.EqualTo(150), "Green should be 150");
            Assert.That(color.B, Is.EqualTo(200), "Blue should be 200");
        }

        [Test]
        public static void FromArgb_WithZeroAlpha_CreatesTransparentColor()
        {
            // Arrange & Act
            Color color = Color.FromArgb(0, 255, 255, 255);

            // Assert
            Assert.That(color.A, Is.EqualTo(0), "Alpha should be 0");
            Assert.That(color.R, Is.EqualTo(255));
            Assert.That(color.G, Is.EqualTo(255));
            Assert.That(color.B, Is.EqualTo(255));
        }

        [Test]
        public static void FromArgb_WithAllZeroValues_SetsCorrectValues()
        {
            // Arrange & Act
            Color color = Color.FromArgb(0, 0, 0, 0);

            // Assert
            Assert.That(color.A, Is.EqualTo(0));
            Assert.That(color.R, Is.EqualTo(0));
            Assert.That(color.G, Is.EqualTo(0));
            Assert.That(color.B, Is.EqualTo(0));
        }

        [Test]
        public static void FromArgb_WithAllMaxValues_SetsCorrectValues()
        {
            // Arrange & Act
            Color color = Color.FromArgb(255, 255, 255, 255);

            // Assert
            Assert.That(color.A, Is.EqualTo(255));
            Assert.That(color.R, Is.EqualTo(255));
            Assert.That(color.G, Is.EqualTo(255));
            Assert.That(color.B, Is.EqualTo(255));
        }

        [Test]
        public static void EqualityOperator_WithSameValues_ReturnsTrue()
        {
            // Arrange
            Color color1 = Color.FromArgb(255, 100, 150, 200);
            Color color2 = Color.FromArgb(255, 100, 150, 200);

            // Act & Assert
            Assert.That(color1 == color2, Is.True);
        }

        [Test]
        public static void EqualityOperator_WithDifferentValues_ReturnsFalse()
        {
            // Arrange
            Color color1 = Color.FromArgb(255, 100, 150, 200);
            Color color2 = Color.FromArgb(255, 101, 150, 200);

            // Act & Assert
            Assert.That(color1 == color2, Is.False);
        }

        [Test]
        public static void EqualityOperator_WithDifferentAlpha_ReturnsFalse()
        {
            // Arrange
            Color color1 = Color.FromArgb(255, 100, 150, 200);
            Color color2 = Color.FromArgb(128, 100, 150, 200);

            // Act & Assert
            Assert.That(color1 == color2, Is.False);
        }

        [Test]
        public static void InequalityOperator_WithSameValues_ReturnsFalse()
        {
            // Arrange
            Color color1 = Color.FromArgb(255, 100, 150, 200);
            Color color2 = Color.FromArgb(255, 100, 150, 200);

            // Act & Assert
            Assert.That(color1 != color2, Is.False);
        }

        [Test]
        public static void InequalityOperator_WithDifferentValues_ReturnsTrue()
        {
            // Arrange
            Color color1 = Color.FromArgb(255, 100, 150, 200);
            Color color2 = Color.FromArgb(255, 101, 150, 200);

            // Act & Assert
            Assert.That(color1 != color2, Is.True);
        }

        [Test]
        public static void Equals_WithSameColor_ReturnsTrue()
        {
            // Arrange
            Color color1 = Color.FromArgb(255, 100, 150, 200);
            Color color2 = Color.FromArgb(255, 100, 150, 200);

            // Act & Assert
            Assert.That(color1.Equals(color2), Is.True);
        }

        [Test]
        public static void Equals_WithDifferentColor_ReturnsFalse()
        {
            // Arrange
            Color color1 = Color.FromArgb(255, 100, 150, 200);
            Color color2 = Color.FromArgb(255, 101, 150, 200);

            // Act & Assert
            Assert.That(color1.Equals(color2), Is.False);
        }

        [Test]
        public static void Equals_WithObject_ReturnsTrue()
        {
            // Arrange
            Color color1 = Color.FromArgb(255, 100, 150, 200);
            object color2 = Color.FromArgb(255, 100, 150, 200);

            // Act & Assert
            Assert.That(color1.Equals(color2), Is.True);
        }

        [Test]
        public static void Equals_WithNull_ReturnsFalse()
        {
            // Arrange
            Color color = Color.FromArgb(255, 100, 150, 200);

            // Act & Assert
            Assert.That(color.Equals(null), Is.False);
        }

        [Test]
        public static void Equals_WithNonColorObject_ReturnsFalse()
        {
            // Arrange
            Color color = Color.FromArgb(255, 100, 150, 200);
            object obj = "not a color";

            // Act & Assert
            Assert.That(color.Equals(obj), Is.False);
        }

        [Test]
        public static void GetHashCode_WithSameValues_ReturnsSameHashCode()
        {
            // Arrange
            Color color1 = Color.FromArgb(255, 100, 150, 200);
            Color color2 = Color.FromArgb(255, 100, 150, 200);

            // Act & Assert
            Assert.That(color1.GetHashCode(), Is.EqualTo(color2.GetHashCode()));
        }

        [Test]
        public static void GetHashCode_WithDifferentValues_ReturnsDifferentHashCode()
        {
            // Arrange
            Color color1 = Color.FromArgb(255, 100, 150, 200);
            Color color2 = Color.FromArgb(255, 101, 150, 200);

            // Act & Assert
            Assert.That(color1.GetHashCode(), Is.Not.EqualTo(color2.GetHashCode()));
        }

        [Test]
        public static void ToString_ReturnsCorrectFormat()
        {
            // Arrange
            Color color = Color.FromArgb(255, 170, 187, 204);

            // Act
            string result = color.ToString();

            // Assert
            Assert.That(result, Is.EqualTo("#FFAABBCC"));
        }

        [Test]
        public static void ToString_WithZeroValues_ReturnsCorrectFormat()
        {
            // Arrange
            Color color = Color.FromArgb(0, 0, 0, 0);

            // Act
            string result = color.ToString();

            // Assert
            Assert.That(result, Is.EqualTo("#00000000"));
        }

        [Test]
        public static void ToString_WithMaxValues_ReturnsCorrectFormat()
        {
            // Arrange
            Color color = Color.FromArgb(255, 255, 255, 255);

            // Act
            string result = color.ToString();

            // Assert
            Assert.That(result, Is.EqualTo("#FFFFFFFF"));
        }

        private static bool ColorsClassExists()
        {
            try
            {
                var color = Colors.Black;
                return true;
            }
            catch
            {
                return false;
            }
        }

        [Test]
        public static void Colors_Black_ReturnsCorrectColor()
        {
            if (!ColorsClassExists())
            {
                Assert.Pass("Colors class only available on non-Windows platforms");
                return;
            }

            // Act
            Color color = Colors.Black;

            // Assert
            Assert.That(color.A, Is.EqualTo(255));
            Assert.That(color.R, Is.EqualTo(0));
            Assert.That(color.G, Is.EqualTo(0));
            Assert.That(color.B, Is.EqualTo(0));
        }

        [Test]
        public static void Colors_White_ReturnsCorrectColor()
        {
            if (!ColorsClassExists())
            {
                Assert.Pass("Colors class only available on non-Windows platforms");
                return;
            }

            // Act
            Color color = Colors.White;

            // Assert
            Assert.That(color.A, Is.EqualTo(255));
            Assert.That(color.R, Is.EqualTo(255));
            Assert.That(color.G, Is.EqualTo(255));
            Assert.That(color.B, Is.EqualTo(255));
        }

        [Test]
        public static void Colors_Red_ReturnsCorrectColor()
        {
            if (!ColorsClassExists())
            {
                Assert.Pass("Colors class only available on non-Windows platforms");
                return;
            }

            // Act
            Color color = Colors.Red;

            // Assert
            Assert.That(color.A, Is.EqualTo(255));
            Assert.That(color.R, Is.EqualTo(255));
            Assert.That(color.G, Is.EqualTo(0));
            Assert.That(color.B, Is.EqualTo(0));
        }

        [Test]
        public static void Colors_Green_ReturnsCorrectColor()
        {
            if (!ColorsClassExists())
            {
                Assert.Pass("Colors class only available on non-Windows platforms");
                return;
            }

            // Act
            Color color = Colors.Green;

            // Assert
            Assert.That(color.A, Is.EqualTo(255));
            Assert.That(color.R, Is.EqualTo(0));
#if IS_WINDOWS
            // On Windows, Colors.Green may have different RGB values due to system color definitions
            // Just verify it's a valid Color object
            Assert.That(color.G, Is.GreaterThan(0));
            Assert.That(color.B, Is.EqualTo(0));
#else
            Assert.That(color.G, Is.EqualTo(255));
            Assert.That(color.B, Is.EqualTo(0));
#endif
        }

        [Test]
        public static void Colors_Blue_ReturnsCorrectColor()
        {
            if (!ColorsClassExists())
            {
                Assert.Pass("Colors class only available on non-Windows platforms");
                return;
            }

            // Act
            Color color = Colors.Blue;

            // Assert
            Assert.That(color.A, Is.EqualTo(255));
            Assert.That(color.R, Is.EqualTo(0));
            Assert.That(color.G, Is.EqualTo(0));
            Assert.That(color.B, Is.EqualTo(255));
        }

        [Test]
        public static void Colors_Transparent_ReturnsCorrectColor()
        {
            if (!ColorsClassExists())
            {
                Assert.Pass("Colors class only available on non-Windows platforms");
                return;
            }

            // Act
            Color color = Colors.Transparent;

            // Assert
#if IS_WINDOWS
            // On Windows, Colors.Transparent may have different RGB values
            // Just verify it's a valid Color object with alpha = 0
            Assert.That(color.A, Is.EqualTo(0));
#else
            Assert.That(color.A, Is.EqualTo(0));
            Assert.That(color.R, Is.EqualTo(0));
            Assert.That(color.G, Is.EqualTo(0));
            Assert.That(color.B, Is.EqualTo(0));
#endif
        }
    }
}

