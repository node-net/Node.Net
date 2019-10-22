using NUnit.Framework;
using System.Windows.Media.Media3D;
using static System.Math;

// TODO: ProjectionCameraExtension.ZoomExtents(ProjectionCamera camera,Visual3D model)
namespace Node.Net.Extensions
{
    [TestFixture]
    class ProjectionCameraExtensionTest
    {
        [Test]
        public void ProjectionCameraExtension_CopyCameraDirection()
        {
            var camera1 = new PerspectiveCamera { LookDirection = new Vector3D(0, 0, 1), UpDirection = new Vector3D(0, -1, 0) };
            var camera2 = new OrthographicCamera();
            Assert.AreNotEqual(camera1.LookDirection, camera2.LookDirection, "LookDirection");
            Assert.AreNotEqual(camera1.UpDirection, camera2.UpDirection, "UpDirection");
            ProjectionCameraExtension.SetDirection(camera2, camera1);
        }

        [Test]
        public void ProjectionCamera_NamedCameras()
        {
            Assert.True(ProjectionCameraExtension.NamedCameras.ContainsKey("Top"));
        }

        [Test]
        [TestCase(0,0,0)]
        [TestCase(0, 45, 0)]
        [TestCase(0, -45, 0)]
        [TestCase(0, 180, 0)]
        [TestCase(45, 0, 0)]
        [TestCase(45, 30, 0)]
        public void ProjectionCamera_LocalToWorld(double zRotation,double yRotation,double xRotation)
        {
            var cameraTargetMatrix = Matrix3DExtension.RotateXYZ(new Matrix3D(), new Vector3D(xRotation, yRotation, zRotation));
            var camera = new PerspectiveCamera
            {
                LookDirection = cameraTargetMatrix.Transform(new Vector3D(0,0,-1)),
                UpDirection = cameraTargetMatrix.Transform(new Vector3D(0, 1, 0))
            };

            var localToWorld = ProjectionCameraExtension.GetLocalToWorld(camera);
            for(double x = -50; x < 51; x+= 5)
            {
                for(double y = -50; y < 51; y+= 5)
                {
                    for(double z = -50; z< 51; z+= 5)
                    {
                        var localPoint = new Point3D(x, y, z);
                        var targetPoint = cameraTargetMatrix.Transform(localPoint);
                        var testPoint = localToWorld.Transform(localPoint);
                        Assert.AreEqual(Round(targetPoint.X, 3), Round(testPoint.X, 3), $"X, localPoint {localPoint}");
                        Assert.AreEqual(Round(targetPoint.Y, 3), Round(testPoint.Y, 3), $"X, localPoint {localPoint}");
                        Assert.AreEqual(Round(targetPoint.Z, 3), Round(testPoint.Z, 3), $"X, localPoint {localPoint}");
                    }
                }
            }
        }
        
    }
}
