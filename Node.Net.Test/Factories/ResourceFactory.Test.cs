using NUnit.Framework;
using System.IO;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media.Media3D;
using static System.Environment;

namespace Node.Net.Factories.Test
{
    [TestFixture]
    class ResourceFactoryTest
    {
        [Test]
        public void ResourceFactory_ImportManifestResources()
        {
            var factory = new ResourceFactory();
            factory.ImportManifestResources(typeof(ResourceFactoryTest).Assembly);

            var mesh = factory.Create(typeof(MeshGeometry3D), "Cube") as MeshGeometry3D;
            Assert.NotNull(mesh, nameof(mesh));

            var cone = factory.Create(typeof(MeshGeometry3D), "Cone") as MeshGeometry3D;
            Assert.NotNull(cone, nameof(cone));
        }

        [Test]
        public void ResourceFactory_ImportResources()
        {
            var resourceDictionary = GlobalFixture.Read("ResourceDictionary.Example.xaml") as ResourceDictionary;
            Assert.NotNull(resourceDictionary);

            var factory = new ResourceFactory();
            factory.ImportResources(resourceDictionary);

            var mesh = factory.Create(typeof(MeshGeometry3D), "Cube") as MeshGeometry3D;
            Assert.NotNull(mesh, nameof(mesh));

            var cone = factory.Create(typeof(MeshGeometry3D), "Cone") as MeshGeometry3D;
            Assert.NotNull(cone, nameof(cone));

            factory.ImportResources($"{GetFolderPath(SpecialFolder.UserProfile)}\\Resources");
        }

        private string GetFileName(string name)
        {
            return $"{GetFolderPath(SpecialFolder.Desktop)}\\{name}";
        }

        [Test]
        public void ResourceFactory_CreateSample()
        {
            var resourceDictionary = new ResourceDictionary();
            var builder = new HelixToolkit.Wpf.MeshBuilder();
            builder.AddBox(new Point3D(0, 0, 0.5), 1, 1, 1);
            resourceDictionary.Add("MeshGeometry3D.Cube", builder.ToMesh(true));

            builder = new HelixToolkit.Wpf.MeshBuilder();
            builder.AddCone(new Point3D(0, 0, 0), new Point3D(0, 0, 1), 0.5, true, 20);
            resourceDictionary.Add("MeshGometry3D.Cone", builder.ToMesh(true));
            using (FileStream fs = new FileStream(GetFileName("ResourceDictionary.Example.xaml"), FileMode.Create))
            {
                XamlWriter.Save(resourceDictionary, fs);
            }
        }
    }
}
