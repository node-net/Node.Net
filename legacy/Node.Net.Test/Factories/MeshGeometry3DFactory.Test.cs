using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using System.Windows.Media.Media3D;
using static System.Environment;

namespace Node.Net.Factories.Test
{
    [TestFixture]
    class MeshGeometry3DFactoryTest
    {
        [Test]
        public void MeshGeometry3DFactory_Usage()
        {
            var factory = new MeshGeometry3DFactory();
            Assert.IsNull(factory.Create(null));

            //factory.Add("Cube", XamlReader.Load(typeof(MeshGeometry3DFactoryTest).Assembly.GetManifestResourceStream("Node.Net.Factories.Test.Resources.MeshGeometry3D.Cube.xaml")) as MeshGeometry3D);
            //Assert.IsNotNull(factory.Create("Cube"));
        }
        private string GetFileName(string name)
        {
            return $"{GetFolderPath(SpecialFolder.Desktop)}\\{name}";
        }
        [Test]
        public void MeshGeometry3DFactory_CreateSamples()
        {
            var builder = new HelixToolkit.Wpf.MeshBuilder();
            builder.AddBox(new Point3D(0, 0, 0.5), 1, 1, 1);
            using (FileStream fs = new FileStream(GetFileName("MeshGeometry3D.Cube.xaml"), FileMode.Create))
            {
                XamlWriter.Save(builder.ToMesh(true),fs);
            }

            builder = new HelixToolkit.Wpf.MeshBuilder();
            builder.AddCone(new Point3D(0, 0, 0), new Point3D(0, 0, 1), 0.5, true, 20);
            using (FileStream fs = new FileStream(GetFileName("MeshGeometry3D.Cone.xaml"), FileMode.Create))
            {
                XamlWriter.Save(builder.ToMesh(true), fs);
            }
        }
    }
}
