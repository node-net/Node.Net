using System.Windows.Media.Media3D;

namespace Node.Net
{
    public static class PerspectiveCameraExtension
    {
        public static bool IsVisible(this PerspectiveCamera camera, Point3D point) => Beta.Internal.PerspectiveCameraExtension.IsVisible(camera, point);
        public static bool IsVisible(this PerspectiveCamera camera, Point3D point, double aspectRatio) => Beta.Internal.PerspectiveCameraExtension.IsVisible(camera, point, aspectRatio);

        public static PerspectiveCamera GetTransformedPerspectiveCamera(this PerspectiveCamera camera, Transform3D transform) => Beta.Internal.PerspectiveCameraExtension.GetTransformedPerspectiveCamera(camera, transform);
        public static double GetVerticalFieldOfView(this PerspectiveCamera camera, double width, double height)
        {
            if (height > 0 && width > 0) return camera.FieldOfView * (height / width);
            return camera.FieldOfView;
        }
        public static OrthographicCamera GetOrthographicCamera(this PerspectiveCamera camera)
        {
            return new OrthographicCamera
            {
                Position = camera.Position,
                LookDirection = camera.LookDirection,
                UpDirection = camera.UpDirection,
                FarPlaneDistance = double.PositiveInfinity,
                NearPlaneDistance = 0.0
            };
        }
    }
}
