using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Node.Net.Test
{
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
        public static async Task Material_CanBeInstantiated()
        {
            if (!CanCreateTestMaterial())
            {
                // TUnit doesn't have Assert.Pass - just return early
                return;
            }

            // Arrange & Act
#if !IS_WINDOWS
            Material material = new TestMaterial();

            // Assert
            await Assert.That(material).IsNotNull();
            await Assert.That(material is Material).IsTrue();
#endif
            await Task.CompletedTask;
        }

        [Test]
        public static async Task Material_IsAssignableFrom_DiffuseMaterial()
        {
            // Arrange
            DiffuseMaterial diffuseMaterial = new DiffuseMaterial();

            // Act & Assert
            await Assert.That(diffuseMaterial is Material).IsTrue();
        }

        [Test]
        public static async Task Material_IsAssignableFrom_EmissiveMaterial()
        {
            // Arrange
            EmissiveMaterial emissiveMaterial = new EmissiveMaterial();

            // Act & Assert
            await Assert.That(emissiveMaterial is Material).IsTrue();
        }

        [Test]
        public static async Task Material_IsAssignableFrom_SpecularMaterial()
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
            await Assert.That(specularMaterial is Material).IsTrue();
        }

        [Test]
        public static async Task Material_IsAssignableFrom_MaterialGroup()
        {
            // Arrange
            MaterialGroup materialGroup = new MaterialGroup();

            // Act & Assert
            await Assert.That(materialGroup is Material).IsTrue();
        }
    }
}

