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
        [TestCase(1,0,0,0,1,0)]
        [TestCase(0, 1, 0, -1, 0, 0)]
        [TestCase(0.86602540378443871, 0.49999999999999994, -1.3877787807814457E-17, -0.492403876506104, 0.85286853195244328, 0.17364817766693036)]
        public void SetDirectionVectorsXY(double xDirX,double xDirY,double xDirZ,double yDirX,double yDirY,double yDirZ)
        {
            var xDirection = new Vector3D(xDirX, xDirY, xDirZ);
            xDirection.Normalize();
            var yDirection = new Vector3D(yDirX, yDirY, yDirZ);
            yDirection.Normalize();

            var zDirection = Vector3D.CrossProduct(xDirection, yDirection);

            var matrix = new Matrix3D();
            matrix = matrix.SetDirectionVectorsXY(xDirection, yDirection);

            var xDirectionCheck = matrix.Transform(new Vector3D(1, 0, 0));
            var yDirectionCheck = matrix.Transform(new Vector3D(0, 1, 0));
            var zDirectionCheck = matrix.Transform(new Vector3D(0, 0, 1));

            Assert.AreEqual(Round(xDirection.X, 3), Round(xDirectionCheck.X, 3), $"xDirection.X");
            Assert.AreEqual(Round(xDirection.Y, 3), Round(xDirectionCheck.Y, 3), $"xDirection.Y");
            Assert.AreEqual(Round(xDirection.Z, 3), Round(xDirectionCheck.Z, 3), $"xDirection.Z");

            Assert.AreEqual(Round(yDirection.X, 3), Round(yDirectionCheck.X, 3), $"yDirection.X");
            Assert.AreEqual(Round(yDirection.Y, 3), Round(yDirectionCheck.Y, 3), $"yDirection.Y");
            Assert.AreEqual(Round(yDirection.Z, 3), Round(yDirectionCheck.Z, 3), $"yDirection.Z");

            Assert.AreEqual(Round(zDirection.X, 3), Round(zDirectionCheck.X, 3), $"zDirection.X");
            Assert.AreEqual(Round(zDirection.Y, 3), Round(zDirectionCheck.Y, 3), $"zDirection.Y");
            Assert.AreEqual(Round(zDirection.Z, 3), Round(zDirectionCheck.Z, 3), $"zDirection.Z");
        }
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
        [TestCase(0,180,0)]
        [TestCase(0, 90, 180)]
        public void SetDirectionVectorsXYZ(double rotationX,double rotationY,double rotationZ)
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

        [Test]
        [TestCase(0, 0, 0)]
        [TestCase(0, 0, 30)]
        //[TestCase(10, 0, 30)]
        public void SetDirectionVectorsZXY(double rotationX, double rotationY, double rotationZ)
        {
            var matrix = new Matrix3D();
            matrix = matrix.RotateZXY(new Vector3D(rotationX, rotationY, rotationZ));

            var xDir = matrix.Transform(new Vector3D(1, 0, 0));
            var yDir = matrix.Transform(new Vector3D(0, 1, 0));
            var zDir = matrix.Transform(new Vector3D(0, 0, 1));

            var matrix2 = new Matrix3D();
            matrix2 = matrix2.SetDirectionVectors(xDir, yDir, zDir);

            var xDir2 = matrix2.Transform(new Vector3D(1, 0, 0));
            var yDir2 = matrix2.Transform(new Vector3D(0, 1, 0));
            var zDir2 = matrix2.Transform(new Vector3D(0, 0, 1));

            Assert.AreEqual(Round(xDir.X, 3), Round(xDir2.X, 3), "xDir.X");
            Assert.AreEqual(Round(xDir.Y, 3), Round(xDir2.Y, 3), "xDir.Y");
            Assert.AreEqual(Round(xDir.Z, 3), Round(xDir2.Z, 3), "xDir.Z");

            var rotationsXYZ = matrix2.GetRotationsZXY();
            Assert.AreEqual(rotationX, Round(rotationsXYZ.X, 2), "rotationX");
            Assert.AreEqual(rotationY, Round(rotationsXYZ.Y, 2), "rotationY");
            Assert.AreEqual(rotationZ, Round(rotationsXYZ.Z, 2), "rotationZ");


        }


        [Test]
        public void GetDictionary()
        {
            var matrix = new Matrix3D();
            matrix.Rotate(new Quaternion(new Vector3D(0, 0, 1), 45));

            var dictionary = matrix.GetDictionary();
            Assert.AreEqual("45 deg", dictionary["RotationZ"]);
        }
    }
}
