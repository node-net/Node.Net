using System.Collections.Generic;
using System.Windows.Media.Media3D;

namespace Node.Net.Extensions
{
    public static class ProjectionCameraExtension
    {
        public static void SetDirection(ProjectionCamera target, ProjectionCamera source)
        {
            target.LookDirection = source.LookDirection;
            target.UpDirection = source.UpDirection;
        }

       

        private static Dictionary<string, ProjectionCamera> namedCameras;
        public static Dictionary<string, ProjectionCamera> NamedCameras
        {
            get
            {
                if (namedCameras == null)
                {
                    namedCameras = new Dictionary<string, ProjectionCamera>();
                    namedCameras.Add("Top",
                        new PerspectiveCamera
                        {
                            LookDirection = new Vector3D(0, 0, 1),
                            UpDirection = new Vector3D(0, 1, 0)
                        });
                    namedCameras.Add("Bottom",
                        new PerspectiveCamera
                        {
                            LookDirection = new Vector3D(0, 0, 1),
                            UpDirection = new Vector3D(0, -1, 0)
                        });
                    namedCameras.Add("Front",
                        new PerspectiveCamera
                        {
                            LookDirection = new Vector3D(0, 1, 0),
                            UpDirection = new Vector3D(0, 0, 1)
                        });
                    namedCameras.Add("Back",
                        new PerspectiveCamera
                        {
                            LookDirection = new Vector3D(0, -1, 0),
                            UpDirection = new Vector3D(0, 0, 1)
                        });
                    namedCameras.Add("Left",
                        new PerspectiveCamera
                        {
                            LookDirection = new Vector3D(1, 0, 0),
                            UpDirection = new Vector3D(0, 0, 1)
                        });
                    namedCameras.Add("Right",
                        new PerspectiveCamera
                        {
                            LookDirection = new Vector3D(-1, 0, 0),
                            UpDirection = new Vector3D(0, 0, 1)
                        });
                    namedCameras.Add("SW",
                        new PerspectiveCamera
                        {
                            LookDirection = new Vector3D(1, 1, -1),
                            UpDirection = new Vector3D(1, 1, 1)
                        });
                    namedCameras.Add("NW",
                        new PerspectiveCamera
                        {
                            LookDirection = new Vector3D(1, -1, -1),
                            UpDirection = new Vector3D(1, -1, 1)
                        });
                    namedCameras.Add("NE",
                        new PerspectiveCamera
                        {
                            LookDirection = new Vector3D(-1, -1, -1),
                            UpDirection = new Vector3D(-1, -1, 1)
                        });
                    namedCameras.Add("SE",
                        new PerspectiveCamera
                        {
                            LookDirection = new Vector3D(-1, 1, -1),
                            UpDirection = new Vector3D(-1, 1, 1)
                        });
                }
                return namedCameras;


            }
        }
    }
}
