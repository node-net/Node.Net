using System;
using System.Threading.Tasks;

namespace Node.Net.Test
{
    internal static class DiffuseMaterialTests
    {
        [Test]
        public static async Task DiffuseMaterial_Constructor_Default_InitializesCorrectly()
        {
            // Arrange & Act
            DiffuseMaterial material = new DiffuseMaterial();

            // Assert
            await Assert.That(material).IsNotNull();
            await Assert.That(material.Brush).IsNull();
        }

        [Test]
        public static async Task DiffuseMaterial_Constructor_WithBrush_InitializesCorrectly()
        {
            // Arrange
            SolidColorBrush brush = new SolidColorBrush(Colors.Red);

            // Act
            DiffuseMaterial material = new DiffuseMaterial(brush);

            // Assert
            await Assert.That(material).IsNotNull();
            await Assert.That(material.Brush).IsEqualTo(brush);
        }

        [Test]
        public static async Task DiffuseMaterial_Brush_CanBeSet()
        {
            // Arrange
            DiffuseMaterial material = new DiffuseMaterial();
            SolidColorBrush brush = new SolidColorBrush(Colors.Blue);

            // Act
            material.Brush = brush;

            // Assert
            await Assert.That(material.Brush).IsEqualTo(brush);
        }

        [Test]
        public static async Task DiffuseMaterial_Brush_CanBeSetToNull()
        {
            // Arrange
            DiffuseMaterial material = new DiffuseMaterial(new SolidColorBrush(Colors.Red));

            // Act
            material.Brush = null;

            // Assert
            await Assert.That(material.Brush).IsNull();
        }

        [Test]
        public static async Task DiffuseMaterial_Brush_CanBeChanged()
        {
            // Arrange
            DiffuseMaterial material = new DiffuseMaterial(new SolidColorBrush(Colors.Red));
            SolidColorBrush newBrush = new SolidColorBrush(Colors.Green);

            // Act
            material.Brush = newBrush;

            // Assert
            await Assert.That(material.Brush).IsEqualTo(newBrush);
            await Assert.That(material.Brush).IsNotEqualTo(new SolidColorBrush(Colors.Red));
        }
    }
}

