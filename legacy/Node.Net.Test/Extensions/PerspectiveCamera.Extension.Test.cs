using NUnit.Framework;
using System.Windows.Media.Media3D;
using static System.Math;

namespace Node.Net.Extensions.Test
{
    class PerspectiveCameraExtensionTest
    {
        [Test]
        [TestCase(0,50,5,true)]
        [TestCase(0,-50,5,false)]
        [TestCase(0,50,20,true)]
        [TestCase(0,50,30,false)]
        public void PerspectiveCamera_IsVisible(double x,double y,double z,bool isVisible)
        {
            var camera = new PerspectiveCamera
            {
                Position = new Point3D(0, 0, 0),
                LookDirection = new Vector3D(0, 1, 0),
                UpDirection = new Vector3D(0, 0, 1)
            };

            Assert.AreEqual(isVisible, PerspectiveCameraExtension.IsVisible(camera, new Point3D(x, y, z)));

            Assert.True(PerspectiveCameraExtension.IsVisible(camera, new Point3D(0, 50, 5)));
            Assert.False(PerspectiveCameraExtension.IsVisible(camera, new Point3D(0, -50, 5)));

            // For 45 degree FieldOfView, the frustum height at 50 meters is 41.42
            Assert.True(PerspectiveCameraExtension.IsVisible(camera, new Point3D(0, 50, 20)));
            Assert.False(PerspectiveCameraExtension.IsVisible(camera, new Point3D(0, 50, 30)));
        }

        [Test]
        [TestCase(0, 0, 0, true)]
        public void PerspectiveCamera_LookingAtOrigin_IsVisible(double x, double y, double z, bool isVisible)
        {
            var camera = new PerspectiveCamera
            {
                Position = new Point3D(100, 100, 0),
                LookDirection = new Vector3D(-1, -1, 0),
                UpDirection = new Vector3D(0, 0, 1),
                FieldOfView = 90
            };

            Assert.AreEqual(isVisible, PerspectiveCameraExtension.IsVisible(camera, new Point3D(x, y, z)));
        }


        [Test]
        public void PerspectiveCamera_PerspectiveCameras()
        {
            var cameras = PerspectiveCameraExtension.PerspectiveCameras;
        }

        [Test]
        public void PerspectiveCamera_GetWorldToLocal()
        {
            var camera = new PerspectiveCamera
            {
                Position = new Point3D(100, 100, 0),
                LookDirection = new Vector3D(-1, -1, 0),
                UpDirection = new Vector3D(0, 0, 1),
                FieldOfView = 90
            };

            var localToWorld = ProjectionCameraExtension.GetLocalToWorld(camera);
            var worldToLocal = ProjectionCameraExtension.GetWorldToLocal(camera);

            var test = ProjectionCameraExtension.GetLocalToWorld(camera).Transform(new Point3D(0, 0, 0));

            var local = ProjectionCameraExtension.GetWorldToLocal(camera).Transform(new Point3D(0,0,0));
            Assert.AreEqual(0, Round(local.X,1));
            Assert.AreEqual(0, Round(local.Y,1));
            Assert.AreEqual(-141.4, Round(local.Z, 1));
        }
    }
}
