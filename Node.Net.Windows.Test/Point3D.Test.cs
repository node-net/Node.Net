using NUnit.Framework;
using System.Windows.Media.Media3D;

namespace Node.Net.Windows
{
	[TestFixture]
    public class Point3DTest
    {
        [Test]
        public void Usage()
        {
			var point = new Point3D(1, 2, 3);
			Assert.AreEqual(3, point.Z);
        }
    }
}