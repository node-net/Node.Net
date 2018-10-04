using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Node.Net.Test.Extension
{
	[TestFixture]
	class Point3DExtensionTest
	{
		[Test]
		public void ParsePoints()
		{
			var points = Point3DExtension.ParsePoints("0,0,0 0,0,1");
			Assert.AreEqual(2, points.Length);
			var points2D = points.Get2DPoints();
			Assert.AreEqual(2, points2D.Length);
			var tpoints = points.Transform(new System.Windows.Media.Media3D.Matrix3D());
			Assert.AreEqual(2, tpoints.Length);
		}
	}
}
