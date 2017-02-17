using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using static System.Math;

namespace Node.Net.Factories.Test.Factories.Helpers
{
    [TestFixture]
    class IMatrix3DHelperTest
    {
        [Test]
        public void IMatrix3DTiltAndSpin()
        {
            var d = new Dictionary<string, dynamic>();
            d["Tilt"] = "-25 deg";
            var matrix = Node.Net.Factories.Deprecated.Factories.Helpers.IMatrix3DHelper.FromIDictionary(d, null);
            var parentNegativeZDir = matrix.Matrix3D.Transform(new Vector3D(0, 0, -1));
            Assert.True(parentNegativeZDir.Z < 0);  // make sure -Z is still below horizon
            var parentXDir = matrix.Matrix3D.Transform(new Vector3D(1, 0, 0));
            Assert.True(parentXDir.Z > 0);          // make sure local X axis is above the horizon
            
            
            d["Spin"] = "-180 deg";
            matrix = Node.Net.Factories.Deprecated.Factories.Helpers.IMatrix3DHelper.FromIDictionary(d, null);
            parentNegativeZDir = matrix.Matrix3D.Transform(new Vector3D(0, 0, -1));
            Assert.True(parentNegativeZDir.Z > 0);  // make sure -Z is now ABOVE the horizon
            var parentXDir2 = matrix.Matrix3D.Transform(new Vector3D(1, 0, 0));
            Assert.True(parentXDir2.Z > 0);          // make sure local X axis is STILL above the horizon
            Assert.AreEqual(Round(parentXDir.X,3), Round(parentXDir2.X,3));
            Assert.AreEqual(Round(parentXDir.Y, 3), Round(parentXDir2.Y, 3));
            Assert.AreEqual(Round(parentXDir.Z, 3), Round(parentXDir2.Z, 3));
        }
    }
}
