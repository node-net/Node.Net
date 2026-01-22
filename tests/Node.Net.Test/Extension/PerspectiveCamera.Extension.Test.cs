#if IS_WINDOWS
using System.Threading.Tasks;
using System.Windows.Media.Media3D; // Use framework types for net8.0-windows
using Node.Net; // For extension methods

namespace Node.Net.Test.Extension
{
    internal class PerspectiveCameraExtensionTest
    {
        [Test]
        public async Task IsVisible()
        {
            PerspectiveCamera camera = new PerspectiveCamera
            {
                Position = new Point3D(0, 0, 100),
                LookDirection = new Vector3D(0, 0, -1),
                UpDirection = new Vector3D(0, 1, 0)
            };
            // Use reflection to access conditionally compiled extension methods
            var assembly = typeof(Factory).Assembly;
            var extensionType = assembly.GetType("Node.Net.PerspectiveCameraExtension");
            if (extensionType == null)
            {
                // TUnit doesn't have Assert.Pass - just return early
                return;
            }
            var isVisibleMethod1 = extensionType.GetMethod("IsVisible", new[] { typeof(PerspectiveCamera), typeof(Point3D) });
            var isVisibleMethod2 = extensionType.GetMethod("IsVisible", new[] { typeof(PerspectiveCamera), typeof(Point3D), typeof(double) });
            await Assert.That((bool)isVisibleMethod1.Invoke(null, new object[] { camera, new Point3D(0, 0, 0) })).IsTrue();
            await Assert.That((bool)isVisibleMethod2.Invoke(null, new object[] { camera, new Point3D(0, 0, 0), .8 })).IsTrue();
        }

        [Test]
        public async Task GetVerticalFieldOfView()
        {
            PerspectiveCamera camera = new PerspectiveCamera
            {
                Position = new Point3D(0, 0, 100),
                LookDirection = new Vector3D(0, 0, -1),
                UpDirection = new Vector3D(0, 1, 0)
            };
            var assembly = typeof(Factory).Assembly;
            var extensionType = assembly.GetType("Node.Net.PerspectiveCameraExtension");
            if (extensionType == null)
            {
                // TUnit doesn't have Assert.Pass - just return early
                return;
            }
            var getVerticalFieldOfViewMethod = extensionType.GetMethod("GetVerticalFieldOfView", new[] { typeof(PerspectiveCamera), typeof(double), typeof(double) });
            await Assert.That((double)getVerticalFieldOfViewMethod.Invoke(null, new object[] { camera, 100.0, 100.0 })).IsEqualTo(45.0);
        }
    }
}
#endif