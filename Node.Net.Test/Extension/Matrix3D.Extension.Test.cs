using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using static System.Math;

namespace Node.Net
{
    [TestFixture]
    class Matrix3DExtensionTest
    {
        [Test]
        public void RotateXYZ_GetRotations()
        {

            Vector3D[] testVectors =
            {
                new Vector3D(0.0,15.0,45.0),

                new Vector3D(0.0, 0.0, 0.0),
                new Vector3D(0.0,0.0,45.0),
                new Vector3D(0.0,0.0,-45.0),
                new Vector3D(0.0,0.0,180.0),

                new Vector3D(0.0,45.0,0.0),
                new Vector3D(0.0,-45.0,0.0),
                new Vector3D(0.0,180.0,0.0),

                new Vector3D(45.0,0.0,0.0),
                new Vector3D(-45.0,0.0,0.0),
                new Vector3D(180.0,0.0,0.0),

                new Vector3D(0.0,15.0,45.0),
                new Vector3D(0.0,-15.0,45.0),
                new Vector3D(0.0,-15.0,-45.0),
                new Vector3D(0.0,15.0,-45.0),

                //new Vector3D(10.0,45.0,0.0)

               // new Vector3D(10.0,15.0,45.0),

            };
            foreach(Vector3D rotations in testVectors)
            {
                var matrix = new Matrix3D();
                var rotated = matrix.RotateXYZ(rotations);

                var rotationZ = rotated.GetRotationZ();
                Assert.AreEqual(Round(rotations.Z, 3), Round(rotationZ, 3), $"GetRotationsZ {rotations}");

                var rotationY = rotated.GetRotationY();
                Assert.AreEqual(Round(rotations.Y, 3), Round(rotationY, 3),  $"GetRotationsY {rotations}");

                var rotationX = rotated.GetRotationX();
                Assert.AreEqual(Round(rotations.Z, 3), Round(rotationZ, 3), $"GetRotationsZ {rotations}");

                var rotations2 = rotated.GetRotationsXYZ();
                Assert.AreEqual(Round(rotations.X, 3), Round(rotations2.X, 3), $"rotation X {rotations}");
                Assert.AreEqual(Round(rotations.Y, 3), Round(rotations2.Y, 3), $"rotation Y {rotations}");
                Assert.AreEqual(Round(rotations.Z, 3), Round(rotations2.Z, 3), $"rotation Z {rotations}");
            }
            
        }
    }
}
