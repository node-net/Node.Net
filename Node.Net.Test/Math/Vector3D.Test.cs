using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Node.Net.Math
{
	internal class Vector3DTest
	{
		[Test]
		public void Usage()
		{
			var vector0 = new Vector3D();
			Assert.AreEqual(0, vector0.X, "vector0.X");
			Assert.AreEqual(0, vector0.Y, "vector0.Y");
			Assert.AreEqual(0, vector0.Z, "vector0.Z");

			var vector1 = new Vector3D(1, 2, 3);
			Assert.AreEqual(1, vector1.X, "vector1.X");
			Assert.AreEqual(2, vector1.Y, "vector1.Y");
			Assert.AreEqual(3, vector1.Z, "vector1.Z");
		}
	}
}
