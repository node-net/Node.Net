using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using static System.Math;

namespace Node.Net.Beta.Internal
{
    public static class PerspectiveCameraExtension
    {
        public static bool IsVisible(this PerspectiveCamera camera, Point3D worldPoint)
        {
            var local = ProjectionCameraExtension.GetWorldToLocal(camera).Transform(worldPoint);
            // camera lookdirection is along -Z axis
            if (local.Z >= 0.0) return false;
            const double deg2rad = 0.01745329;
            var distance = Abs(local.Z);
            var frustrumHeight = 2.0 * distance * Tan(camera.FieldOfView * deg2rad * 0.5);
            if (Abs(local.X) > frustrumHeight / 2.0) return false;
            if (Abs(local.Y) > frustrumHeight / 2.0) return false;
            return true;
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
    }
}
