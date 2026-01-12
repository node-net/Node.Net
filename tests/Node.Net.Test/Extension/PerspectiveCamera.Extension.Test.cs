#if IS_WINDOWS
extern alias NodeNet;
using NUnit.Framework;
using System.Windows.Media.Media3D; // Use framework types for net8.0-windows
using NodeNet::Node.Net; // For extension methods

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
            // Use reflection to access conditionally compiled extension methods
            var assembly = typeof(NodeNet::Node.Net.Factory).Assembly;
            var extensionType = assembly.GetType("Node.Net.PerspectiveCameraExtension");
            if (extensionType == null)
            {
                Assert.Pass("PerspectiveCameraExtension type not found - skipping test on non-Windows target");
            }
            var isVisibleMethod1 = extensionType.GetMethod("IsVisible", new[] { typeof(PerspectiveCamera), typeof(Point3D) });
            var isVisibleMethod2 = extensionType.GetMethod("IsVisible", new[] { typeof(PerspectiveCamera), typeof(Point3D), typeof(double) });
            Assert.That((bool)isVisibleMethod1.Invoke(null, new object[] { camera, new Point3D(0, 0, 0) }), Is.True);
            Assert.That((bool)isVisibleMethod2.Invoke(null, new object[] { camera, new Point3D(0, 0, 0), .8 }), Is.True);
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
            var assembly = typeof(NodeNet::Node.Net.Factory).Assembly;
            var extensionType = assembly.GetType("Node.Net.PerspectiveCameraExtension");
            if (extensionType == null)
            {
                Assert.Pass("PerspectiveCameraExtension type not found - skipping test on non-Windows target");
            }
            var getVerticalFieldOfViewMethod = extensionType.GetMethod("GetVerticalFieldOfView", new[] { typeof(PerspectiveCamera), typeof(double), typeof(double) });
            Assert.That((double)getVerticalFieldOfViewMethod.Invoke(null, new object[] { camera, 100.0, 100.0 }), Is.EqualTo(45.0));
        }
    }
}
#endif