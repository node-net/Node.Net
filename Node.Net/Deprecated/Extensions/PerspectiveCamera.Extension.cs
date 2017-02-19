using System.Collections.Generic;
using System.Windows.Media.Media3D;
using static System.Math;

namespace Node.Net.Deprecated.Extensions
{
    public static class PerspectiveCameraExtension
    {
        public static PerspectiveCamera GetTransformedPerspectiveCamera(PerspectiveCamera camera, Transform3D transform)
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
        private static Dictionary<string, PerspectiveCamera> perspectiveCameras = null;
        public static Dictionary<string, PerspectiveCamera> PerspectiveCameras
        {
            get
            {
                if (perspectiveCameras == null)
                {
                    perspectiveCameras = new Dictionary<string, PerspectiveCamera>();
                    perspectiveCameras.Add("Plan",
                        new PerspectiveCamera
                        {
                            Position = new Point3D(0, 0, 50),
                            LookDirection = new Vector3D(0, 0, -1),
                            UpDirection = new Vector3D(0, 1, 0)
                        });
                    perspectiveCameras.Add("Default", perspectiveCameras["Plan"]);
                    perspectiveCameras.Add("Top", perspectiveCameras["Plan"]);
                    perspectiveCameras.Add("Front", 
                        new PerspectiveCamera
                        {
                            Position = new Point3D(0, -50, 0),
                            LookDirection = new Vector3D(0, 1, 0),
                            UpDirection = new Vector3D(0, 0, 1)
                        });
                    perspectiveCameras.Add("Right",
                        new PerspectiveCamera
                        {
                            Position = new Point3D(0, -50, 0),
                            LookDirection = new Vector3D(0, 1, 0),
                            UpDirection = new Vector3D(0, 0, 1)
                        });
                }
                return perspectiveCameras;
            }
        }

        public static bool IsVisible(PerspectiveCamera camera, Point3D worldPoint)
        {
            var local = ProjectionCameraExtension.GetWorldToLocal(camera).Transform(worldPoint);
            // camera lookdirection is along -Z axis
            if (local.Z >= 0.0) return false;
            var deg2rad = 0.01745329;
            var distance = Abs(local.Z);
            var frustrumHeight = 2.0 * distance * Tan(camera.FieldOfView*deg2rad*0.5);
            if (Abs(local.X) > frustrumHeight / 2.0) return false;
            if(Abs(local.Y) > frustrumHeight/2.0) return false;
            return true;
        }
    }
}
