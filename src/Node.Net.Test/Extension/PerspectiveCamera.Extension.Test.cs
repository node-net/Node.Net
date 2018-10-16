using NUnit.Framework;
using System.Windows.Media.Media3D;

namespace Node.Net.Test.Extension
{
	[TestFixture]
	internal class PerspectiveCameraExtensionTest
	{
		[Test]
		public void IsVisible()
		{
			var camera = new PerspectiveCamera
			{
				Position = new Point3D(0, 0, 100),
				LookDirection = new Vector3D(0, 0, -1),
				UpDirection = new Vector3D(0, 1, 0)
			};
			Assert.True(camera.IsVisible(new Point3D(0, 0, 0)));
			Assert.True(camera.IsVisible(new Point3D(0, 0, 0), .8));
		}

		[Test]
		public void GetVerticalFieldOfView()
		{
			var camera = new PerspectiveCamera
			{
				Position = new Point3D(0, 0, 100),
				LookDirection = new Vector3D(0, 0, -1),
				UpDirection = new Vector3D(0, 1, 0)
			};
			Assert.AreEqual(45.0, camera.GetVerticalFieldOfView(100, 100));
		}
	}
}