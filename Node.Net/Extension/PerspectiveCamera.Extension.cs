using System.Windows.Media.Media3D;

namespace Node.Net
{
    public static class PerspectiveCameraExtension
    {
        public static bool IsVisible(this PerspectiveCamera camera, Point3D point) => Beta.Internal.PerspectiveCameraExtension.IsVisible(camera, point);
        public static PerspectiveCamera GetTransformedPerspectiveCamera(this PerspectiveCamera camera, Transform3D transform) => Beta.Internal.PerspectiveCameraExtension.GetTransformedPerspectiveCamera(camera, transform);
    }
}
