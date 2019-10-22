using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using static System.Math;

namespace Node.Net.Factories.Test
{
    [TestFixture]
    class Transform3DFactoryTest
    {
        [Test]
        public void Transform3DFactory_Usage()
        {
            var factory = new Transform3DFactory();
            var dictionary = new Dictionary<string, dynamic>();
            dictionary["Orientation"] = "45 deg";
            var transform = factory.Create(typeof(Transform3D), dictionary) as Transform3D;
            var point = transform.Transform(new Point3D(1, 0, 0));
            Assert.AreEqual(0.7071, Round(point.X, 4),"point.X");
            Assert.AreEqual(0.7071, Round(point.Y,4),"point.Y");
            Assert.AreEqual(0.0, Round(point.Z, 4), "point.Z");
        }
    }
}
