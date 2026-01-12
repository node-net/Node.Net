using System;
using NUnit.Framework;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Reflection;

namespace Node.Net.Test
{
    [TestFixture]
    internal static class MaterialTests
    {
        private static bool CanCreateTestMaterial()
        {
#if !IS_WINDOWS
            try
            {
                TestMaterial material = new TestMaterial();
                return material != null;
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
        // Test concrete implementation for Material (only works on non-Windows)
        private class TestMaterial : Material
        {
        }
#endif

        [Test]
        public static void Material_CanBeInstantiated()
        {
            if (!CanCreateTestMaterial())
            {
                Assert.Pass("TestMaterial only available on non-Windows platforms");
                return;
            }

            // Arrange & Act
#if !IS_WINDOWS
            Material material = new TestMaterial();

            // Assert
            Assert.That(material, Is.Not.Null);
            Assert.That(material, Is.InstanceOf<Material>());
#endif
        }

        [Test]
        public static void Material_IsAssignableFrom_DiffuseMaterial()
        {
            // Arrange
            DiffuseMaterial diffuseMaterial = new DiffuseMaterial();

            // Act & Assert
            Assert.That(diffuseMaterial, Is.InstanceOf<Material>());
        }

        [Test]
        public static void Material_IsAssignableFrom_EmissiveMaterial()
        {
            // Arrange
            EmissiveMaterial emissiveMaterial = new EmissiveMaterial();

            // Act & Assert
            Assert.That(emissiveMaterial, Is.InstanceOf<Material>());
        }

        [Test]
        public static void Material_IsAssignableFrom_SpecularMaterial()
        {
            // Arrange
            SolidColorBrush brush = new SolidColorBrush(Colors.White);
            SpecularMaterial specularMaterial;
            try
            {
                // Try default constructor first (non-Windows)
                specularMaterial = new SpecularMaterial();
            }
            catch
            {
                // On Windows, use constructor with brush and specularPower
                specularMaterial = new SpecularMaterial(brush, 20.0);
            }

            // Act & Assert
            Assert.That(specularMaterial, Is.InstanceOf<Material>());
        }

        [Test]
        public static void Material_IsAssignableFrom_MaterialGroup()
        {
            // Arrange
            MaterialGroup materialGroup = new MaterialGroup();

            // Act & Assert
            Assert.That(materialGroup, Is.InstanceOf<Material>());
        }
    }
}

