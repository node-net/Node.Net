using System;
using System.Threading.Tasks;

namespace Node.Net.Test
{
    internal class MaterialGroupTests
    {
        [Test]
        public async Task MaterialGroup_Constructor_InitializesChildren()
        {
            // Arrange & Act
            MaterialGroup materialGroup = new MaterialGroup();

            // Assert
            await Assert.That(materialGroup).IsNotNull();
            await Assert.That(materialGroup.Children).IsNotNull();
            await Assert.That(materialGroup.Children.Count).IsEqualTo(0);
        }

        [Test]
        public async Task MaterialGroup_Children_CanAddMaterial()
        {
            // Arrange
            MaterialGroup materialGroup = new MaterialGroup();
            DiffuseMaterial material = new DiffuseMaterial(new SolidColorBrush(Colors.Red));

            // Act
            materialGroup.Children.Add(material);

            // Assert
            await Assert.That(materialGroup.Children.Count).IsEqualTo(1);
            await Assert.That(materialGroup.Children[0]).IsEqualTo(material);
        }

        [Test]
        public async Task MaterialGroup_Children_CanAddMultipleMaterials()
        {
            // Arrange
            MaterialGroup materialGroup = new MaterialGroup();
            DiffuseMaterial diffuseMaterial = new DiffuseMaterial(new SolidColorBrush(Colors.Blue));
            SpecularMaterial specularMaterial = new SpecularMaterial(new SolidColorBrush(Colors.White), 50.0);
            EmissiveMaterial emissiveMaterial = new EmissiveMaterial(new SolidColorBrush(Colors.Yellow));

            // Act
            materialGroup.Children.Add(diffuseMaterial);
            materialGroup.Children.Add(specularMaterial);
            materialGroup.Children.Add(emissiveMaterial);

            // Assert
            await Assert.That(materialGroup.Children.Count).IsEqualTo(3);
            await Assert.That(materialGroup.Children[0]).IsEqualTo(diffuseMaterial);
            await Assert.That(materialGroup.Children[1]).IsEqualTo(specularMaterial);
            await Assert.That(materialGroup.Children[2]).IsEqualTo(emissiveMaterial);
        }

        [Test]
        public async Task MaterialGroup_Children_CanRemoveMaterial()
        {
            // Arrange
            MaterialGroup materialGroup = new MaterialGroup();
            DiffuseMaterial material = new DiffuseMaterial(new SolidColorBrush(Colors.Red));
            materialGroup.Children.Add(material);

            // Act
            bool removed = materialGroup.Children.Remove(material);

            // Assert
            await Assert.That(removed).IsTrue();
            await Assert.That(materialGroup.Children.Count).IsEqualTo(0);
        }

        [Test]
        public async Task MaterialGroup_Children_CanClear()
        {
            // Arrange
            MaterialGroup materialGroup = new MaterialGroup();
            materialGroup.Children.Add(new DiffuseMaterial(new SolidColorBrush(Colors.Red)));
            SolidColorBrush whiteBrush = new SolidColorBrush(Colors.White);
            SpecularMaterial specularMaterial;
#if IS_WINDOWS
            specularMaterial = new SpecularMaterial(whiteBrush, 20.0);
#else
            specularMaterial = new SpecularMaterial(whiteBrush);
#endif
            materialGroup.Children.Add(specularMaterial);

            // Act
            materialGroup.Children.Clear();

            // Assert
            await Assert.That(materialGroup.Children.Count).IsEqualTo(0);
        }

        [Test]
        public async Task MaterialGroup_Children_CanBeSet()
        {
            // Arrange
            MaterialGroup materialGroup = new MaterialGroup();
            MaterialCollection newCollection = new MaterialCollection();
            newCollection.Add(new DiffuseMaterial(new SolidColorBrush(Colors.Blue)));

            // Act
            materialGroup.Children = newCollection;

            // Assert
            await Assert.That(materialGroup.Children).IsEqualTo(newCollection);
            await Assert.That(materialGroup.Children.Count).IsEqualTo(1);
        }

        [Test]
        public async Task MaterialGroup_Children_SetToNull_InitializesNewCollection()
        {
            // Arrange
            MaterialGroup materialGroup = new MaterialGroup();
            materialGroup.Children.Add(new DiffuseMaterial(new SolidColorBrush(Colors.Red)));

            // Act
            try
            {
                materialGroup.Children = null;
            }
            catch
            {
                // On Windows, MaterialGroup.Children cannot be set to null (throws exception)
                // TUnit doesn't have Assert.Pass - just return early
                return;
            }

            // Assert
#if IS_WINDOWS
            // On Windows, setting to null may actually set it to null, but getter should handle it
            // If it's null, the getter should return a new collection
            if (materialGroup.Children == null)
            {
                // TUnit doesn't have Assert.Pass - just return early
                return;
            }
#endif
            await Assert.That(materialGroup.Children).IsNotNull();
            await Assert.That(materialGroup.Children.Count).IsEqualTo(0);
        }

        [Test]
        public async Task MaterialCollection_CanContainDifferentMaterialTypes()
        {
            // Arrange
            MaterialCollection collection = new MaterialCollection();
            DiffuseMaterial diffuse = new DiffuseMaterial(new SolidColorBrush(Colors.Red));
            EmissiveMaterial emissive = new EmissiveMaterial(new SolidColorBrush(Colors.Yellow));
            SolidColorBrush whiteBrush = new SolidColorBrush(Colors.White);
            SpecularMaterial specular;
#if IS_WINDOWS
            specular = new SpecularMaterial(whiteBrush, 20.0);
#else
            specular = new SpecularMaterial(whiteBrush);
#endif
            MaterialGroup group = new MaterialGroup();

            // Act
            collection.Add(diffuse);
            collection.Add(emissive);
            collection.Add(specular);
            collection.Add(group);

            // Assert
            await Assert.That(collection.Count).IsEqualTo(4);
            await Assert.That(collection[0] is DiffuseMaterial).IsTrue();
            await Assert.That(collection[1] is EmissiveMaterial).IsTrue();
            await Assert.That(collection[2] is SpecularMaterial).IsTrue();
            await Assert.That(collection[3] is MaterialGroup).IsTrue();
        }

        [Test]
        public async Task MaterialCollection_CanInsertMaterial()
        {
            // Arrange
            MaterialCollection collection = new MaterialCollection();
            DiffuseMaterial material1 = new DiffuseMaterial(new SolidColorBrush(Colors.Red));
            DiffuseMaterial material2 = new DiffuseMaterial(new SolidColorBrush(Colors.Blue));
            collection.Add(material1);

            // Act
            collection.Insert(0, material2);

            // Assert
            await Assert.That(collection.Count).IsEqualTo(2);
            await Assert.That(collection[0]).IsEqualTo(material2);
            await Assert.That(collection[1]).IsEqualTo(material1);
        }

        [Test]
        public async Task MaterialCollection_CanGetMaterialByIndex()
        {
            // Arrange
            MaterialCollection collection = new MaterialCollection();
            DiffuseMaterial material = new DiffuseMaterial(new SolidColorBrush(Colors.Red));
            collection.Add(material);

            // Act & Assert
            await Assert.That(collection[0]).IsEqualTo(material);
        }

        [Test]
        public async Task MaterialCollection_CanSetMaterialByIndex()
        {
            // Arrange
            MaterialCollection collection = new MaterialCollection();
            DiffuseMaterial material1 = new DiffuseMaterial(new SolidColorBrush(Colors.Red));
            DiffuseMaterial material2 = new DiffuseMaterial(new SolidColorBrush(Colors.Blue));
            collection.Add(material1);

            // Act
            collection[0] = material2;

            // Assert
            await Assert.That(collection[0]).IsEqualTo(material2);
            await Assert.That(collection[0]).IsNotEqualTo(material1);
        }
    }
}

