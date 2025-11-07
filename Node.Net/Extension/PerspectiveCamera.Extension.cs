#if IS_WINDOWS
using System.Windows.Media.Media3D;
using static System.Math;

namespace Node.Net
{
    public static class PerspectiveCameraExtension
    {
        public static bool IsVisible(this PerspectiveCamera camera, Point3D worldPoint)
        {
            try
            {
                Point3D local = ProjectionCameraExtension.GetWorldToLocal(camera).Transform(worldPoint);
                // camera lookdirection is along -Z axis
                if (local.Z >= 0.0)
                {
                    return false;
                }

                const double deg2rad = 0.01745329;
                double distance = Abs(local.Z);
                double frustrumHeight = 2.0 * distance * Tan(camera.FieldOfView * deg2rad * 0.5);
                if (Abs(local.X) > frustrumHeight / 2.0)
                {
                    return false;
                }

                if (Abs(local.Y) > frustrumHeight / 2.0)
                {
                    return false;
                }

                return true;
            }
            catch { return false; }
        }

        public static bool IsVisible(this PerspectiveCamera camera, Point3D worldPoint, double aspectRatio)
        {
            try
            {
                Point3D local = ProjectionCameraExtension.GetWorldToLocal(camera).Transform(worldPoint);
                // camera lookdirection is along -Z axis
                if (local.Z >= 0.0)
                {
                    return false;
                }

                const double deg2rad = 0.01745329;
                double distance = Abs(local.Z);

                double horizontalFOV = camera.FieldOfView;
                double verticalFOV = camera.FieldOfView;
                if (aspectRatio >= 1)
                {
                    verticalFOV = horizontalFOV / aspectRatio;
                }
                else
                {
                    horizontalFOV = verticalFOV * aspectRatio;
                }
                double frustrumWidth = 2.0 * distance * Tan(horizontalFOV * deg2rad * 0.5);
                double frustrumHeight = 2.0 * distance * Tan(verticalFOV * deg2rad * 0.5);
                if (Abs(local.X) > frustrumWidth / 2.0)
                {
                    return false;
                }

                if (Abs(local.Y) > frustrumHeight / 2.0)
                {
                    return false;
                }

                return true;
            }
            catch { return false; }
        }

        public static PerspectiveCamera GetTransformedPerspectiveCamera(this PerspectiveCamera camera, Transform3D transform)
        {
            return new PerspectiveCamera
            {
                LookDirection = transform.Transform(camera.LookDirection),
                UpDirection = transform.Transform(camera.UpDirection),
                FieldOfView = camera.FieldOfView,
                FarPlaneDistance = camera.FarPlaneDistance,
                NearPlaneDistance = camera.NearPlaneDistance,
                Position = transform.Transform(camera.Position)
            };
        }

        public static double GetVerticalFieldOfView(this PerspectiveCamera camera, double width, double height)
        {
            if (height > 0 && width > 0)
            {
                return camera.FieldOfView * (height / width);
            }

            return camera.FieldOfView;
        }
    }
}
#endif