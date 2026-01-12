extern alias NodeNet;
using System;
using NUnit.Framework;
using NodeNet::System.Windows.Media;
using NodeNet::System.Windows.Media.Media3D;

namespace Node.Net.Test
{
    [TestFixture]
    internal static class EmissiveMaterialTests
    {
        [Test]
        public static void EmissiveMaterial_Constructor_Default_InitializesCorrectly()
        {
            // Arrange & Act
            EmissiveMaterial material = new EmissiveMaterial();

            // Assert
            Assert.That(material, Is.Not.Null);
            Assert.That(material.Brush, Is.Null);
        }

        [Test]
        public static void EmissiveMaterial_Constructor_WithBrush_InitializesCorrectly()
        {
            // Arrange
            SolidColorBrush brush = new SolidColorBrush(Colors.Yellow);

            // Act
            EmissiveMaterial material = new EmissiveMaterial(brush);

            // Assert
            Assert.That(material, Is.Not.Null);
            Assert.That(material.Brush, Is.EqualTo(brush));
        }

        [Test]
        public static void EmissiveMaterial_Brush_CanBeSet()
        {
            // Arrange
            EmissiveMaterial material = new EmissiveMaterial();
            SolidColorBrush brush = new SolidColorBrush(Colors.Cyan);

            // Act
            material.Brush = brush;

            // Assert
            Assert.That(material.Brush, Is.EqualTo(brush));
        }

        [Test]
        public static void EmissiveMaterial_Brush_CanBeSetToNull()
        {
            // Arrange
            EmissiveMaterial material = new EmissiveMaterial(new SolidColorBrush(Colors.Yellow));

            // Act
            material.Brush = null;

            // Assert
            Assert.That(material.Brush, Is.Null);
        }

        [Test]
        public static void EmissiveMaterial_Brush_CanBeChanged()
        {
            // Arrange
            EmissiveMaterial material = new EmissiveMaterial(new SolidColorBrush(Colors.Yellow));
            SolidColorBrush newBrush = new SolidColorBrush(Colors.Magenta);

            // Act
            material.Brush = newBrush;

            // Assert
            Assert.That(material.Brush, Is.EqualTo(newBrush));
            Assert.That(material.Brush, Is.Not.EqualTo(new SolidColorBrush(Colors.Yellow)));
        }
    }
}

