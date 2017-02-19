using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using static System.Math;

namespace Node.Net.Deprecated.Extensions
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

        public static Matrix3D GetCameraMatrix3D(Point3D position,Vector3D lookDirection,Vector3D upDirection)
        {
            var matrix = new Matrix3D();
            //matrix.Translate(new Vector3D(position.X, position.Y, position.Z));
            // LookDirection = Local -Z Axis
            // UpDirection   = Local Y Axis
            var zAngle = Vector3D.AngleBetween(lookDirection, new Vector3D(0, 0, -1));
            if (zAngle != 0.0)
            {
                var normal = Vector3D.CrossProduct(lookDirection, new Vector3D(0, 0, -1));
                matrix.Rotate(new Quaternion(normal, zAngle));
                if(Round(matrix.Transform(new Vector3D(0,0,-1)).Z,4) != -1.0)
                {
                    matrix = new Matrix3D();
                   // matrix.Translate(new Vector3D(position.X, position.Y, position.Z));
                    matrix.Rotate(new Quaternion(normal, -zAngle));
                }

                
            }
            var localYAxis = matrix.Transform(new Vector3D(0, 1, 0));
            var yAngle = Vector3D.AngleBetween(upDirection, localYAxis);
            {
                if (Round(yAngle, 5) != 0.0)
                {
                    var normal = Vector3D.CrossProduct(upDirection, localYAxis);
                    matrix.Rotate(new Quaternion(normal, yAngle));
                    if (Round(matrix.Transform(localYAxis).Y, 4) != Round(localYAxis.Y,4))
                    {
                        matrix.Rotate(new Quaternion(normal, yAngle*-2.0));
                    }
                }
            }

            var localNegZAxis = matrix.Transform(new Vector3D(0, 0, -1));
            zAngle = Vector3D.AngleBetween(lookDirection, localNegZAxis);
            if (Round(zAngle,5) != 0.0)
            {
                var normal = Vector3D.CrossProduct(lookDirection, localNegZAxis);
                matrix.Rotate(new Quaternion(normal, zAngle));
                if (Round(matrix.Transform(localNegZAxis).Z, 4) != Round(localNegZAxis.Z,4))
                {
                    matrix.Rotate(new Quaternion(normal, zAngle*-2.0));
                }
            }

            matrix.Translate(new Vector3D(position.X, position.Y, position.Z));
            return matrix;
        }

        public static Matrix3D GetLocalToWorld(ProjectionCamera camera)
        {
            return GetCameraMatrix3D(camera.Position,camera.LookDirection, camera.UpDirection);
        }
        public static Matrix3D GetWorldToLocal(ProjectionCamera camera)
        {
            var tmp = GetLocalToWorld(camera);
            tmp.Invert();
            return tmp;
        }
    }
}
