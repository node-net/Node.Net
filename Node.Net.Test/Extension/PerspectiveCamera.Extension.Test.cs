#if IS_WINDOWS
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
            PerspectiveCamera camera = new PerspectiveCamera
            {
                Position = new Point3D(0, 0, 100),
                LookDirection = new Vector3D(0, 0, -1),
                UpDirection = new Vector3D(0, 1, 0)
            };
            Assert.That(camera.IsVisible(new Point3D(0, 0, 0)), Is.True);
            Assert.That(camera.IsVisible(new Point3D(0, 0, 0), .8), Is.True);
        }

        [Test]
        public void GetVerticalFieldOfView()
        {
            PerspectiveCamera camera = new PerspectiveCamera
            {
                Position = new Point3D(0, 0, 100),
                LookDirection = new Vector3D(0, 0, -1),
                UpDirection = new Vector3D(0, 1, 0)
            };
            Assert.That(camera.GetVerticalFieldOfView(100, 100),Is.EqualTo(45.0));
        }
    }
}
#endif