using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Node.Net.Math
{
	[TestFixture]
	internal class Point3DTest
	{
		[Test]
		public void Usage()
		{
			var point0 = new Point3D();
			Assert.AreEqual(0, point0.X, "point0.X");
			Assert.AreEqual(0, point0.Y, "point0.Y");
			Assert.AreEqual(0, point0.Z, "point0.Z");

			var point1 = new Point3D(1,2,3);
			Assert.AreEqual(1, point1.X, "point1.X");
			Assert.AreEqual(2, point1.Y, "point1.Y");
			Assert.AreEqual(3, point1.Z, "point1.Z");
		}
	}
}
