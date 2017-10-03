using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using NUnit.Framework;

namespace Node.Net.Extension
{
    [TestFixture,NUnit.Framework.Category(nameof(Vector3DExtension))]
    class Vector3DExtensionTest
    {
        [Test]
        public void GetPerpendicular()
        {
            var perpendicular = new Vector3D(0, 0, 1).GetPerpendicular();
            Assert.AreEqual(1, perpendicular.X, "perpendicular.X");

            perpendicular = new Vector3D(1, 0, 0).GetPerpendicular();
            Assert.AreEqual(1, perpendicular.Y, "perpendicular.Y");

            perpendicular = new Vector3D(0, 1, 0).GetPerpendicular();
            Assert.AreEqual(-1, perpendicular.X, "perpendicular.X");
        }
    }
}
