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
    [TestFixture,Category("Matrix3D")]
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

        [Test]
        [TestCase(0,0,0)]
        [TestCase(0,0,45)]
        [TestCase(0, 0, -45)]
        [TestCase(0,45,0)]
        [TestCase(0, -45, 0)]
        [TestCase(0,45,5)]
        public void SetDirectionVectors(double rotationX,double rotationY,double rotationZ)
        {
            var matrix = new Matrix3D();
            matrix = matrix.RotateXYZ(new Vector3D(rotationX, rotationY, rotationZ));

            var xDir = matrix.Transform(new Vector3D(1, 0, 0));
            var yDir = matrix.Transform(new Vector3D(0, 1, 0));
            var zDir = matrix.Transform(new Vector3D(0, 0, 1));

            var matrix2 = new Matrix3D();
            matrix2 = matrix2.SetDirectionVectors(xDir, yDir, zDir);

            var xDir2 = matrix2.Transform(new Vector3D(1, 0, 0));
            var yDir2 = matrix2.Transform(new Vector3D(0, 1, 0));
            var zDir2 = matrix2.Transform(new Vector3D(0, 0, 1));

            Assert.AreEqual(Round(xDir.X,3), Round(xDir2.X,3),   "xDir.X");
            Assert.AreEqual(Round(xDir.Y, 3), Round(xDir2.Y, 3), "xDir.Y");
            Assert.AreEqual(Round(xDir.Z, 3), Round(xDir2.Z, 3), "xDir.Z");
            
            var rotationsXYZ = matrix2.GetRotationsXYZ();
            Assert.AreEqual(rotationX, Round(rotationsXYZ.X,2), "rotationX");
            Assert.AreEqual(rotationY, Round(rotationsXYZ.Y,2), "rotationY");
            Assert.AreEqual(rotationZ, Round(rotationsXYZ.Z,2), "rotationZ");
            

        }
    }
}
