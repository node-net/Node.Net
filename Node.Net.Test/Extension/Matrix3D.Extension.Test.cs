﻿using NUnit.Framework;
using System.Windows.Media.Media3D;
using static System.Math;

namespace Node.Net.Extension
{
    [TestFixture]
    internal static class Matrix3DExtensionTest
    {
        [Test]
        [TestCase(0, 0, 0)]
        [TestCase(45, 30, 0)]
        [TestCase(45, 30, 0)]
        public static void RotateOTS(double orientation, double tilt, double spin)
        {
            Matrix3D matrix = new Matrix3D();
            matrix = matrix.RotateOTS(new Vector3D(orientation, tilt, spin));
            Vector3D ots = matrix.GetRotationsOTS();
            Assert.AreEqual(orientation, Round(ots.X, 2), "orientation");
            Assert.AreEqual(tilt, Round(ots.Y, 2), "tilt");
            Assert.AreEqual(spin, Round(ots.Z, 2), "spin");
        }
    }
}