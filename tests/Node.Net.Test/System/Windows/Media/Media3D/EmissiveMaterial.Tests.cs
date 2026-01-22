using System;
using System.Threading.Tasks;

namespace Node.Net.Test
{
    internal class EmissiveMaterialTests
    {
        [Test]
        public async Task EmissiveMaterial_Constructor_Default_InitializesCorrectly()
        {
            // Arrange & Act
            EmissiveMaterial material = new EmissiveMaterial();

            // Assert
            await Assert.That(material).IsNotNull();
            await Assert.That(material.Brush).IsNull();
        }

        [Test]
        public async Task EmissiveMaterial_Constructor_WithBrush_InitializesCorrectly()
        {
            // Arrange
            SolidColorBrush brush = new SolidColorBrush(Colors.Yellow);

            // Act
            EmissiveMaterial material = new EmissiveMaterial(brush);

            // Assert
            await Assert.That(material).IsNotNull();
            await Assert.That(material.Brush).IsEqualTo(brush);
        }

        [Test]
        public async Task EmissiveMaterial_Brush_CanBeSet()
        {
            // Arrange
            EmissiveMaterial material = new EmissiveMaterial();
            SolidColorBrush brush = new SolidColorBrush(Colors.Cyan);

            // Act
            material.Brush = brush;

            // Assert
            await Assert.That(material.Brush).IsEqualTo(brush);
        }

        [Test]
        public async Task EmissiveMaterial_Brush_CanBeSetToNull()
        {
            // Arrange
            EmissiveMaterial material = new EmissiveMaterial(new SolidColorBrush(Colors.Yellow));

            // Act
            material.Brush = null;

            // Assert
            await Assert.That(material.Brush).IsNull();
        }

        [Test]
        public async Task EmissiveMaterial_Brush_CanBeChanged()
        {
            // Arrange
            EmissiveMaterial material = new EmissiveMaterial(new SolidColorBrush(Colors.Yellow));
            SolidColorBrush newBrush = new SolidColorBrush(Colors.Magenta);

            // Act
            material.Brush = newBrush;

            // Assert
            await Assert.That(material.Brush).IsEqualTo(newBrush);
            await Assert.That(material.Brush).IsNotEqualTo(new SolidColorBrush(Colors.Yellow));
        }
    }
}

