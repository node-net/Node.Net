using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Node.Net.Extensions
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
                    perspectiveCameras.Add("Top", perspectiveCameras["Default"]);
                }
                return perspectiveCameras;
            }
        }
    }
}
