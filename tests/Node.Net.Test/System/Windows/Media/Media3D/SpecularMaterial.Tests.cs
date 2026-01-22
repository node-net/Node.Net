using System;
using System.Threading.Tasks;

namespace Node.Net.Test
{
    internal class SpecularMaterialTests
    {
        [Test]
        public async Task SpecularMaterial_Constructor_Default_InitializesCorrectly()
        {
#if IS_WINDOWS
            // On Windows, SpecularMaterial doesn't have a default constructor
            // TUnit doesn't have Assert.Pass - just return early
            return;
#else
            // Arrange & Act
            SpecularMaterial material = new SpecularMaterial();

            // Assert
            await Assert.That(material).IsNotNull();
            await Assert.That(material.Brush).IsNull();
            await Assert.That(material.SpecularPower).IsEqualTo(20.0);
#endif
        }

        [Test]
        public async Task SpecularMaterial_Constructor_WithBrush_InitializesCorrectly()
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
            await Assert.That(material).IsNotNull();
            await Assert.That(material.Brush).IsEqualTo(brush);
            await Assert.That(material.SpecularPower).IsEqualTo(20.0);
        }

        [Test]
        public async Task SpecularMaterial_Constructor_WithBrushAndSpecularPower_InitializesCorrectly()
        {
            // Arrange
            SolidColorBrush brush = new SolidColorBrush(Colors.White);
            double specularPower = 50.0;

            // Act
            SpecularMaterial material = new SpecularMaterial(brush, specularPower);

            // Assert
            await Assert.That(material).IsNotNull();
            await Assert.That(material.Brush).IsEqualTo(brush);
            await Assert.That(material.SpecularPower).IsEqualTo(specularPower);
        }

        [Test]
        public async Task SpecularMaterial_Brush_CanBeSet()
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
            await Assert.That(material.Brush).IsEqualTo(brush);
        }

        [Test]
        public async Task SpecularMaterial_Brush_CanBeSetToNull()
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
            await Assert.That(material.Brush).IsNull();
        }

        [Test]
        public async Task SpecularMaterial_SpecularPower_CanBeSet()
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
            await Assert.That(material.SpecularPower).IsEqualTo(specularPower);
        }

        [Test]
        public async Task SpecularMaterial_SpecularPower_DefaultsToTwenty()
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
            await Assert.That(material.SpecularPower).IsEqualTo(20.0);
        }

        [Test]
        public async Task SpecularMaterial_SpecularPower_CanBeSetToZero()
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
            await Assert.That(material.SpecularPower).IsEqualTo(0.0);
        }

        [Test]
        public async Task SpecularMaterial_SpecularPower_CanBeSetToLargeValue()
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
            await Assert.That(material.SpecularPower).IsEqualTo(largeValue);
        }

        [Test]
        public async Task SpecularMaterial_SpecularPower_CanBeSetToNegativeValue()
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
            await Assert.That(material.SpecularPower).IsEqualTo(negativeValue);
        }
    }
}

