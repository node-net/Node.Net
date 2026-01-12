extern alias NodeNet;
using System;
using NUnit.Framework;
using NodeNet::System.Windows.Media;
using NodeNet::System.Windows.Media.Media3D;

namespace Node.Net.Test
{
    [TestFixture]
    internal static class DiffuseMaterialTests
    {
        [Test]
        public static void DiffuseMaterial_Constructor_Default_InitializesCorrectly()
        {
            // Arrange & Act
            DiffuseMaterial material = new DiffuseMaterial();

            // Assert
            Assert.That(material, Is.Not.Null);
            Assert.That(material.Brush, Is.Null);
        }

        [Test]
        public static void DiffuseMaterial_Constructor_WithBrush_InitializesCorrectly()
        {
            // Arrange
            SolidColorBrush brush = new SolidColorBrush(Colors.Red);

            // Act
            DiffuseMaterial material = new DiffuseMaterial(brush);

            // Assert
            Assert.That(material, Is.Not.Null);
            Assert.That(material.Brush, Is.EqualTo(brush));
        }

        [Test]
        public static void DiffuseMaterial_Brush_CanBeSet()
        {
            // Arrange
            DiffuseMaterial material = new DiffuseMaterial();
            SolidColorBrush brush = new SolidColorBrush(Colors.Blue);

            // Act
            material.Brush = brush;

            // Assert
            Assert.That(material.Brush, Is.EqualTo(brush));
        }

        [Test]
        public static void DiffuseMaterial_Brush_CanBeSetToNull()
        {
            // Arrange
            DiffuseMaterial material = new DiffuseMaterial(new SolidColorBrush(Colors.Red));

            // Act
            material.Brush = null;

            // Assert
            Assert.That(material.Brush, Is.Null);
        }

        [Test]
        public static void DiffuseMaterial_Brush_CanBeChanged()
        {
            // Arrange
            DiffuseMaterial material = new DiffuseMaterial(new SolidColorBrush(Colors.Red));
            SolidColorBrush newBrush = new SolidColorBrush(Colors.Green);

            // Act
            material.Brush = newBrush;

            // Assert
            Assert.That(material.Brush, Is.EqualTo(newBrush));
            Assert.That(material.Brush, Is.Not.EqualTo(new SolidColorBrush(Colors.Red)));
        }
    }
}

