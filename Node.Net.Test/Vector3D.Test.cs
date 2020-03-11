using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Windows.Media.Media3D;
using NUnit.Framework;
using static System.Math;

namespace Node.Net.Test
{
    [TestFixture]
    static class Vector3DExtensionTest
    {
        [Test]
        public static void ComputeRayPlaneIntersection()
        {
            var intersection = Vector3DExtension.ComputeRayPlaneIntersection(new Vector3D(0, 0, -1), new Vector3D(0, 0, 10), new Vector3D(0, 0, 1), new Vector3D(0, 0, 0));
            Assert.AreEqual(0, Round(intersection.X, 4), "intersection.X");
            Assert.AreEqual(0, Round(intersection.Y, 4), "intersection.Y");
            Assert.AreEqual(0, Round(intersection.Z, 4), "intersection.Z");

            intersection = Vector3DExtension.ComputeRayPlaneIntersection(new Vector3D(0, 0, -1), new Vector3D(0, 0, 0), new Vector3D(0, 0, 1), new Vector3D(0, 0, 0));
            Assert.AreEqual(0, Round(intersection.X, 4), "intersection.X");
            Assert.AreEqual(0, Round(intersection.Y, 4), "intersection.Y");
            Assert.AreEqual(0, Round(intersection.Z, 4), "intersection.Z");

            intersection = Vector3DExtension.ComputeRayPlaneIntersection(new Vector3D(0, 0.5, -1), new Vector3D(0, 0, 10), new Vector3D(0, 0, 1), new Vector3D(0, 0, 0));
            Assert.AreEqual(0, Round(intersection.X, 4), "intersection.X");
            Assert.AreEqual(5.0, Round(intersection.Y, 4), "intersection.Y");
            Assert.AreEqual(0, Round(intersection.Z, 4), "intersection.Z");

            // ray facing away from plane
            intersection = Vector3DExtension.ComputeRayPlaneIntersection(new Vector3D(0, -0.5, 1), new Vector3D(0, 0, 10), new Vector3D(0, 0, 1), new Vector3D(0, 0, 0));
            Assert.AreEqual(0, Round(intersection.X, 4), "intersection.X");
            Assert.AreEqual(5.0, Round(intersection.Y, 4), "intersection.Y");
            Assert.AreEqual(0, Round(intersection.Z, 4), "intersection.Z");
        }

        [Test]
        public static void GetTheta()
        {
            Assert.AreEqual(-45.0, Round(new Vector3D(0.5, -0.5, -1.0).GetTheta(), 4), "ray.GetTheta()");
            Assert.AreEqual(0, Round(new Vector3D(0.5, 0, -1.0).GetTheta(), 4), "ray.GetTheta()");
            Assert.AreEqual(45.0, Round(new Vector3D(0.5,0.5,-1.0).GetTheta(), 4), "ray.GetTheta()");
            Assert.AreEqual(90.0, Round(new Vector3D(0, 0.5, -1.0).GetTheta(), 4), "ray.GetTheta()");
            Assert.AreEqual(135.0, Round(new Vector3D(-0.5, 0.5, -1.0).GetTheta(), 4), "ray.GetTheta()");
        }

        [Test]
        public static void GetPhi()
        {
            Assert.AreEqual(0.0, Round(new Vector3D(0, 0, 1.0).GetPhi(), 4), "ray.GetPhi()");
            Assert.AreEqual(45.0, Round(new Vector3D(1.0, 0, 1.0).GetPhi(), 4), "ray.GetPhi()");
            Assert.AreEqual(90.0, Round(new Vector3D(1.0, 0, 0.0).GetPhi(), 4), "ray.GetPhi()");
            Assert.AreEqual(135.0, Round(new Vector3D(1.0, 0, -1.0).GetPhi(), 4), "ray.GetPhi()");
            Assert.AreEqual(180.0, Round(new Vector3D(0, 0, -1.0).GetPhi(), 4), "ray.GetPhi()");
        }
    }
}
