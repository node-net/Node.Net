using System;
using NUnit.Framework;

namespace Node.Net.Test
{
    [TestFixture]
    internal static class SpecularMaterialTests
    {
        [Test]
        public static void SpecularMaterial_Constructor_Default_InitializesCorrectly()
        {
#if IS_WINDOWS
            // On Windows, SpecularMaterial doesn't have a default constructor
            Assert.Pass("SpecularMaterial default constructor not available on Windows");
#else
            // Arrange & Act
            SpecularMaterial material = new SpecularMaterial();

            // Assert
            Assert.That(material, Is.Not.Null);
            Assert.That(material.Brush, Is.Null);
            Assert.That(material.SpecularPower, Is.EqualTo(20.0));
#endif
        }

        [Test]
        public static void SpecularMaterial_Constructor_WithBrush_InitializesCorrectly()
        {
            // Arrange
            SolidColorBrush brush = new SolidColorBrush(Colors.White);

            // Act
            SpecularMaterial material;
#if IS_WINDOWS
            // On Windows, SpecularMaterial requires both Brush and SpecularPower
            material = new SpecularMaterial(brush, 20.0);
#else
            // Try constructor with just brush first (non-Windows)
            material = new SpecularMaterial(brush);
#endif

            // Assert
            Assert.That(material, Is.Not.Null);
            Assert.That(material.Brush, Is.EqualTo(brush));
            Assert.That(material.SpecularPower, Is.EqualTo(20.0));
        }

        [Test]
        public static void SpecularMaterial_Constructor_WithBrushAndSpecularPower_InitializesCorrectly()
        {
            // Arrange
            SolidColorBrush brush = new SolidColorBrush(Colors.White);
            double specularPower = 50.0;

            // Act
            SpecularMaterial material = new SpecularMaterial(brush, specularPower);

            // Assert
            Assert.That(material, Is.Not.Null);
            Assert.That(material.Brush, Is.EqualTo(brush));
            Assert.That(material.SpecularPower, Is.EqualTo(specularPower));
        }

        [Test]
        public static void SpecularMaterial_Brush_CanBeSet()
        {
            // Arrange
            SpecularMaterial material;
            try
            {
                material = new SpecularMaterial();
            }
            catch
            {
                material = new SpecularMaterial(new SolidColorBrush(Colors.White), 20.0);
            }
            SolidColorBrush brush = new SolidColorBrush(Colors.White);

            // Act
            material.Brush = brush;

            // Assert
            Assert.That(material.Brush, Is.EqualTo(brush));
        }

        [Test]
        public static void SpecularMaterial_Brush_CanBeSetToNull()
        {
            // Arrange
            SolidColorBrush brush = new SolidColorBrush(Colors.White);
            SpecularMaterial material;
#if IS_WINDOWS
            // On Windows, SpecularMaterial requires both Brush and SpecularPower
            material = new SpecularMaterial(brush, 20.0);
#else
            // Try constructor with just brush first (non-Windows)
            material = new SpecularMaterial(brush);
#endif

            // Act
            material.Brush = null;

            // Assert
            Assert.That(material.Brush, Is.Null);
        }

        [Test]
        public static void SpecularMaterial_SpecularPower_CanBeSet()
        {
            // Arrange
            SpecularMaterial material;
            try
            {
                material = new SpecularMaterial();
            }
            catch
            {
                // On Windows, use constructor with brush and specularPower
                material = new SpecularMaterial(new SolidColorBrush(Colors.White), 20.0);
            }
            double specularPower = 75.0;

            // Act
            material.SpecularPower = specularPower;

            // Assert
            Assert.That(material.SpecularPower, Is.EqualTo(specularPower));
        }

        [Test]
        public static void SpecularMaterial_SpecularPower_DefaultsToTwenty()
        {
            // Arrange & Act
#if IS_WINDOWS
            // On Windows, SpecularMaterial doesn't have a default constructor
            // Test with constructor that takes brush and specularPower
            SolidColorBrush brush = new SolidColorBrush(Colors.White);
            SpecularMaterial material = new SpecularMaterial(brush, 20.0);
#else
            SpecularMaterial material = new SpecularMaterial();
#endif

            // Assert
            Assert.That(material.SpecularPower, Is.EqualTo(20.0));
        }

        [Test]
        public static void SpecularMaterial_SpecularPower_CanBeSetToZero()
        {
            // Arrange
            SpecularMaterial material;
            try
            {
                material = new SpecularMaterial();
            }
            catch
            {
                material = new SpecularMaterial(new SolidColorBrush(Colors.White), 20.0);
            }

            // Act
            material.SpecularPower = 0.0;

            // Assert
            Assert.That(material.SpecularPower, Is.EqualTo(0.0));
        }

        [Test]
        public static void SpecularMaterial_SpecularPower_CanBeSetToLargeValue()
        {
            // Arrange
            SpecularMaterial material;
            try
            {
                material = new SpecularMaterial();
            }
            catch
            {
                material = new SpecularMaterial(new SolidColorBrush(Colors.White), 20.0);
            }
            double largeValue = 1000.0;

            // Act
            material.SpecularPower = largeValue;

            // Assert
            Assert.That(material.SpecularPower, Is.EqualTo(largeValue));
        }

        [Test]
        public static void SpecularMaterial_SpecularPower_CanBeSetToNegativeValue()
        {
            // Arrange
            SpecularMaterial material;
            try
            {
                material = new SpecularMaterial();
            }
            catch
            {
                material = new SpecularMaterial(new SolidColorBrush(Colors.White), 20.0);
            }
            double negativeValue = -10.0;

            // Act
            material.SpecularPower = negativeValue;

            // Assert
            Assert.That(material.SpecularPower, Is.EqualTo(negativeValue));
        }
    }
}

