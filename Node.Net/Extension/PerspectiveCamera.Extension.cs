using System.Windows.Media.Media3D;

namespace Node.Net
{
    public static class PerspectiveCameraExtension
    {
        public static bool IsVisibleTo(this PerspectiveCamera camera, Point3D point) => Beta.Internal.PerspectiveCameraExtension.IsVisible(camera, point);
    }
}
