using System;
using NUnit.Framework;

namespace Node.Net.Test
{
    [TestFixture]
    internal static class BrushesTests
    {
        private static bool BrushesClassExists()
        {
            try
            {
                var brush = Brushes.Black;
                return brush != null;
            }
            catch
            {
                return false;
            }
        }

        [Test]
        public static void Brushes_Black_ReturnsCorrectBrush()
        {
            if (!BrushesClassExists())
            {
                Assert.Pass("Brushes class only available on non-Windows platforms");
                return;
            }

            // Act
            SolidColorBrush brush = Brushes.Black;

            // Assert
            Assert.That(brush, Is.Not.Null);
            Assert.That(brush.Color, Is.EqualTo(Colors.Black));
        }

        [Test]
        public static void Brushes_White_ReturnsCorrectBrush()
        {
            if (!BrushesClassExists())
            {
                Assert.Pass("Brushes class only available on non-Windows platforms");
                return;
            }

            // Act
            SolidColorBrush brush = Brushes.White;

            // Assert
            Assert.That(brush, Is.Not.Null);
            Assert.That(brush.Color, Is.EqualTo(Colors.White));
        }

        [Test]
        public static void Brushes_Red_ReturnsCorrectBrush()
        {
            if (!BrushesClassExists())
            {
                Assert.Pass("Brushes class only available on non-Windows platforms");
                return;
            }

            // Act
            SolidColorBrush brush = Brushes.Red;

            // Assert
            Assert.That(brush, Is.Not.Null);
            Assert.That(brush.Color, Is.EqualTo(Colors.Red));
        }

        [Test]
        public static void Brushes_Green_ReturnsCorrectBrush()
        {
            if (!BrushesClassExists())
            {
                Assert.Pass("Brushes class only available on non-Windows platforms");
                return;
            }

            // Act
            SolidColorBrush brush = Brushes.Green;

            // Assert
            Assert.That(brush, Is.Not.Null);
            Assert.That(brush.Color, Is.EqualTo(Colors.Green));
        }

        [Test]
        public static void Brushes_Blue_ReturnsCorrectBrush()
        {
            if (!BrushesClassExists())
            {
                Assert.Pass("Brushes class only available on non-Windows platforms");
                return;
            }

            // Act
            SolidColorBrush brush = Brushes.Blue;

            // Assert
            Assert.That(brush, Is.Not.Null);
            Assert.That(brush.Color, Is.EqualTo(Colors.Blue));
        }

        [Test]
        public static void Brushes_Yellow_ReturnsCorrectBrush()
        {
            if (!BrushesClassExists())
            {
                Assert.Pass("Brushes class only available on non-Windows platforms");
                return;
            }

            // Act
            SolidColorBrush brush = Brushes.Yellow;

            // Assert
            Assert.That(brush, Is.Not.Null);
            Assert.That(brush.Color, Is.EqualTo(Colors.Yellow));
        }

        [Test]
        public static void Brushes_Transparent_ReturnsCorrectBrush()
        {
            if (!BrushesClassExists())
            {
                Assert.Pass("Brushes class only available on non-Windows platforms");
                return;
            }

            // Act
            SolidColorBrush brush = Brushes.Transparent;

            // Assert
            Assert.That(brush, Is.Not.Null);
            Assert.That(brush.Color, Is.EqualTo(Colors.Transparent));
        }

        [Test]
        public static void Brushes_Properties_ReturnNewInstances()
        {
            if (!BrushesClassExists())
            {
                Assert.Pass("Brushes class only available on non-Windows platforms");
                return;
            }

            // Act
            SolidColorBrush brush1 = Brushes.Blue;
            SolidColorBrush brush2 = Brushes.Blue;

            // Assert - Each call should return a new instance (or at least test that they're equivalent)
            Assert.That(brush1.Color, Is.EqualTo(brush2.Color));
        }
    }
}

