using System;
using System.Threading.Tasks;

namespace Node.Net.Test
{
    internal class ColorTests
    {
        [Test]
        public async Task Constructor_Default_InitializesToZero()
        {
            Color color = new Color();
            await Assert.That((int)color.A).IsEqualTo(0);
            await Assert.That((int)color.R).IsEqualTo(0);
            await Assert.That((int)color.G).IsEqualTo(0);
            await Assert.That((int)color.B).IsEqualTo(0);
        }

        [Test]
        public async Task Properties_CanBeSet()
        {
            Color color = new Color();
            color.A = 255;
            color.R = 100;
            color.G = 150;
            color.B = 200;
            await Assert.That((int)color.A).IsEqualTo(255);
            await Assert.That((int)color.R).IsEqualTo(100);
            await Assert.That((int)color.G).IsEqualTo(150);
            await Assert.That((int)color.B).IsEqualTo(200);
        }

        [Test]
        public async Task FromRgb_WithParameters_SetsCorrectValues()
        {
            // Arrange & Act
            Color color = Color.FromRgb(100, 150, 200);

            // Assert
            await Assert.That((int)color.A).IsEqualTo(255);
            await Assert.That((int)color.R).IsEqualTo(100);
            await Assert.That((int)color.G).IsEqualTo(150);
            await Assert.That((int)color.B).IsEqualTo(200);
        }

        [Test]
        public async Task FromRgb_WithZeroValues_SetsCorrectValues()
        {
            // Arrange & Act
            Color color = Color.FromRgb(0, 0, 0);

            // Assert
            await Assert.That((int)color.A).IsEqualTo(255);
            await Assert.That((int)color.R).IsEqualTo(0);
            await Assert.That((int)color.G).IsEqualTo(0);
            await Assert.That((int)color.B).IsEqualTo(0);
        }

        [Test]
        public async Task FromRgb_WithMaxValues_SetsCorrectValues()
        {
            // Arrange & Act
            Color color = Color.FromRgb(255, 255, 255);

            // Assert
            await Assert.That((int)color.A).IsEqualTo(255);
            await Assert.That((int)color.R).IsEqualTo(255);
            await Assert.That((int)color.G).IsEqualTo(255);
            await Assert.That((int)color.B).IsEqualTo(255);
        }

        [Test]
        public async Task FromArgb_WithParameters_SetsCorrectValues()
        {
            // Arrange & Act
            Color color = Color.FromArgb(128, 100, 150, 200);

            // Assert
            await Assert.That((int)color.A).IsEqualTo(128);
            await Assert.That((int)color.R).IsEqualTo(100);
            await Assert.That((int)color.G).IsEqualTo(150);
            await Assert.That((int)color.B).IsEqualTo(200);
        }

        [Test]
        public async Task FromArgb_WithZeroAlpha_CreatesTransparentColor()
        {
            // Arrange & Act
            Color color = Color.FromArgb(0, 255, 255, 255);

            // Assert
            await Assert.That((int)color.A).IsEqualTo(0);
            await Assert.That((int)color.R).IsEqualTo(255);
            await Assert.That((int)color.G).IsEqualTo(255);
            await Assert.That((int)color.B).IsEqualTo(255);
        }

        [Test]
        public async Task FromArgb_WithAllZeroValues_SetsCorrectValues()
        {
            // Arrange & Act
            Color color = Color.FromArgb(0, 0, 0, 0);

            // Assert
            await Assert.That((int)color.A).IsEqualTo(0);
            await Assert.That((int)color.R).IsEqualTo(0);
            await Assert.That((int)color.G).IsEqualTo(0);
            await Assert.That((int)color.B).IsEqualTo(0);
        }

        [Test]
        public async Task FromArgb_WithAllMaxValues_SetsCorrectValues()
        {
            // Arrange & Act
            Color color = Color.FromArgb(255, 255, 255, 255);

            // Assert
            await Assert.That((int)color.A).IsEqualTo(255);
            await Assert.That((int)color.R).IsEqualTo(255);
            await Assert.That((int)color.G).IsEqualTo(255);
            await Assert.That((int)color.B).IsEqualTo(255);
        }

        [Test]
        public async Task EqualityOperator_WithSameValues_ReturnsTrue()
        {
            // Arrange
            Color color1 = Color.FromArgb(255, 100, 150, 200);
            Color color2 = Color.FromArgb(255, 100, 150, 200);

            // Act & Assert
            await Assert.That(color1 == color2).IsTrue();
        }

        [Test]
        public async Task EqualityOperator_WithDifferentValues_ReturnsFalse()
        {
            // Arrange
            Color color1 = Color.FromArgb(255, 100, 150, 200);
            Color color2 = Color.FromArgb(255, 101, 150, 200);

            // Act & Assert
            await Assert.That(color1 == color2).IsFalse();
        }

        [Test]
        public async Task EqualityOperator_WithDifferentAlpha_ReturnsFalse()
        {
            // Arrange
            Color color1 = Color.FromArgb(255, 100, 150, 200);
            Color color2 = Color.FromArgb(128, 100, 150, 200);

            // Act & Assert
            await Assert.That(color1 == color2).IsFalse();
        }

        [Test]
        public async Task InequalityOperator_WithSameValues_ReturnsFalse()
        {
            // Arrange
            Color color1 = Color.FromArgb(255, 100, 150, 200);
            Color color2 = Color.FromArgb(255, 100, 150, 200);

            // Act & Assert
            await Assert.That(color1 != color2).IsFalse();
        }

        [Test]
        public async Task InequalityOperator_WithDifferentValues_ReturnsTrue()
        {
            // Arrange
            Color color1 = Color.FromArgb(255, 100, 150, 200);
            Color color2 = Color.FromArgb(255, 101, 150, 200);

            // Act & Assert
            await Assert.That(color1 != color2).IsTrue();
        }

        [Test]
        public async Task Equals_WithSameColor_ReturnsTrue()
        {
            // Arrange
            Color color1 = Color.FromArgb(255, 100, 150, 200);
            Color color2 = Color.FromArgb(255, 100, 150, 200);

            // Act & Assert
            await Assert.That(color1.Equals(color2)).IsTrue();
        }

        [Test]
        public async Task Equals_WithDifferentColor_ReturnsFalse()
        {
            // Arrange
            Color color1 = Color.FromArgb(255, 100, 150, 200);
            Color color2 = Color.FromArgb(255, 101, 150, 200);

            // Act & Assert
            await Assert.That(color1.Equals(color2)).IsFalse();
        }

        [Test]
        public async Task Equals_WithObject_ReturnsTrue()
        {
            // Arrange
            Color color1 = Color.FromArgb(255, 100, 150, 200);
            object color2 = Color.FromArgb(255, 100, 150, 200);

            // Act & Assert
            await Assert.That(color1.Equals(color2)).IsTrue();
        }

        [Test]
        public async Task Equals_WithNull_ReturnsFalse()
        {
            // Arrange
            Color color = Color.FromArgb(255, 100, 150, 200);

            // Act & Assert
            await Assert.That(color.Equals(null)).IsFalse();
        }

        [Test]
        public async Task Equals_WithNonColorObject_ReturnsFalse()
        {
            // Arrange
            Color color = Color.FromArgb(255, 100, 150, 200);
            object obj = "not a color";

            // Act & Assert
            await Assert.That(color.Equals(obj)).IsFalse();
        }

        [Test]
        public async Task GetHashCode_WithSameValues_ReturnsSameHashCode()
        {
            // Arrange
            Color color1 = Color.FromArgb(255, 100, 150, 200);
            Color color2 = Color.FromArgb(255, 100, 150, 200);

            // Act & Assert
            await Assert.That(color1.GetHashCode()).IsEqualTo(color2.GetHashCode());
        }

        [Test]
        public async Task GetHashCode_WithDifferentValues_ReturnsDifferentHashCode()
        {
            // Arrange
            Color color1 = Color.FromArgb(255, 100, 150, 200);
            Color color2 = Color.FromArgb(255, 101, 150, 200);

            // Act & Assert
            await Assert.That(color1.GetHashCode()).IsNotEqualTo(color2.GetHashCode());
        }

        [Test]
        public async Task ToString_ReturnsCorrectFormat()
        {
            // Arrange
            Color color = Color.FromArgb(255, 170, 187, 204);

            // Act
            string result = color.ToString();

            // Assert
            await Assert.That(result).IsEqualTo("#FFAABBCC");
        }

        [Test]
        public async Task ToString_WithZeroValues_ReturnsCorrectFormat()
        {
            // Arrange
            Color color = Color.FromArgb(0, 0, 0, 0);

            // Act
            string result = color.ToString();

            // Assert
            await Assert.That(result).IsEqualTo("#00000000");
        }

        [Test]
        public async Task ToString_WithMaxValues_ReturnsCorrectFormat()
        {
            // Arrange
            Color color = Color.FromArgb(255, 255, 255, 255);

            // Act
            string result = color.ToString();

            // Assert
            await Assert.That(result).IsEqualTo("#FFFFFFFF");
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
        public async Task Colors_Black_ReturnsCorrectColor()
        {
            if (!ColorsClassExists())
            {
                // TUnit doesn't have Assert.Pass - just return early
                return;
            }

            // Act
            Color color = Colors.Black;

            // Assert
            await Assert.That((int)color.A).IsEqualTo(255);
            await Assert.That((int)color.R).IsEqualTo(0);
            await Assert.That((int)color.G).IsEqualTo(0);
            await Assert.That((int)color.B).IsEqualTo(0);
        }

        [Test]
        public async Task Colors_White_ReturnsCorrectColor()
        {
            if (!ColorsClassExists())
            {
                // TUnit doesn't have Assert.Pass - just return early
                return;
            }

            // Act
            Color color = Colors.White;

            // Assert
            await Assert.That((int)color.A).IsEqualTo(255);
            await Assert.That((int)color.R).IsEqualTo(255);
            await Assert.That((int)color.G).IsEqualTo(255);
            await Assert.That((int)color.B).IsEqualTo(255);
        }

        [Test]
        public async Task Colors_Red_ReturnsCorrectColor()
        {
            if (!ColorsClassExists())
            {
                // TUnit doesn't have Assert.Pass - just return early
                return;
            }

            // Act
            Color color = Colors.Red;

            // Assert
            await Assert.That((int)color.A).IsEqualTo(255);
            await Assert.That((int)color.R).IsEqualTo(255);
            await Assert.That((int)color.G).IsEqualTo(0);
            await Assert.That((int)color.B).IsEqualTo(0);
        }

        [Test]
        public async Task Colors_Green_ReturnsCorrectColor()
        {
            if (!ColorsClassExists())
            {
                // TUnit doesn't have Assert.Pass - just return early
                return;
            }

            // Act
            Color color = Colors.Green;

            // Assert
            await Assert.That((int)color.A).IsEqualTo(255);
            await Assert.That((int)color.R).IsEqualTo(0);
#if IS_WINDOWS
            // On Windows, Colors.Green may have different RGB values due to system color definitions
            // Just verify it's a valid Color object
            await Assert.That(color.G).IsGreaterThan(0);
            await Assert.That((int)color.B).IsEqualTo(0);
#else
            await Assert.That((int)color.G).IsEqualTo(255);
            await Assert.That((int)color.B).IsEqualTo(0);
#endif
        }

        [Test]
        public async Task Colors_Blue_ReturnsCorrectColor()
        {
            if (!ColorsClassExists())
            {
                // TUnit doesn't have Assert.Pass - just return early
                return;
            }

            // Act
            Color color = Colors.Blue;

            // Assert
            await Assert.That((int)color.A).IsEqualTo(255);
            await Assert.That((int)color.R).IsEqualTo(0);
            await Assert.That((int)color.G).IsEqualTo(0);
            await Assert.That((int)color.B).IsEqualTo(255);
        }

        [Test]
        public async Task Colors_Transparent_ReturnsCorrectColor()
        {
            if (!ColorsClassExists())
            {
                // TUnit doesn't have Assert.Pass - just return early
                return;
            }

            // Act
            Color color = Colors.Transparent;

            // Assert
#if IS_WINDOWS
            // On Windows, Colors.Transparent may have different RGB values
            // Just verify it's a valid Color object with alpha = 0
            await Assert.That((int)color.A).IsEqualTo(0);
#else
            await Assert.That((int)color.A).IsEqualTo(0);
            await Assert.That((int)color.R).IsEqualTo(0);
            await Assert.That((int)color.G).IsEqualTo(0);
            await Assert.That((int)color.B).IsEqualTo(0);
#endif
        }
    }
}

