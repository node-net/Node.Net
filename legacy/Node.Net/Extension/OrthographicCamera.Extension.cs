using System.Windows.Media.Media3D;

namespace Node.Net
{
    public static class OrthographicCameraExtension
    {
        public static OrthographicCamera GetTransformedCamera(this OrthographicCamera camera, Transform3D transform)
        {
            return new OrthographicCamera
            {
                LookDirection = transform.Transform(camera.LookDirection),
                UpDirection = transform.Transform(camera.UpDirection),
                Position = transform.Transform(camera.Position),
                FarPlaneDistance = camera.FarPlaneDistance,
                NearPlaneDistance = camera.NearPlaneDistance,
                Width = camera.Width
            };
        }
    }
}
