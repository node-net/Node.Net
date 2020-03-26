using System;
using System.Windows.Media.Media3D;
using static System.Math;

namespace Node.Net
{
    public static class ProjectionCameraExtension
    {
        public static Matrix3D GetLocalToWorld(ProjectionCamera camera)
        {
            return GetCameraMatrix3D(camera.Position, camera.LookDirection, camera.UpDirection);
        }

        public static Matrix3D GetWorldToLocal(ProjectionCamera camera)
        {
            Matrix3D tmp = GetLocalToWorld(camera);
            tmp.Invert();
            return tmp;
        }

        public static Matrix3D GetCameraMatrix3D(Point3D position, Vector3D lookDirection, Vector3D upDirection)
        {
            Matrix3D matrix = new Matrix3D();
            double zAngle = Vector3D.AngleBetween(lookDirection, new Vector3D(0, 0, -1));
            if (zAngle != 0.0)
            {
                Vector3D normal = Vector3D.CrossProduct(lookDirection, new Vector3D(0, 0, -1));
                matrix.Rotate(new Quaternion(normal, zAngle));
                if (Round(matrix.Transform(new Vector3D(0, 0, -1)).Z, 4) != -1.0)
                {
                    matrix = new Matrix3D();
                    matrix.Rotate(new Quaternion(normal, -zAngle));
                }
            }
            Vector3D localYAxis = matrix.Transform(new Vector3D(0, 1, 0));
            double yAngle = Vector3D.AngleBetween(upDirection, localYAxis);
            {
                if (Round(yAngle, 5) != 0.0)
                {
                    Vector3D normal = Vector3D.CrossProduct(upDirection, localYAxis);
                    matrix.Rotate(new Quaternion(normal, yAngle));
                    if (Round(matrix.Transform(localYAxis).Y, 4) != Round(localYAxis.Y, 4))
                    {
                        matrix.Rotate(new Quaternion(normal, yAngle * -2.0));
                    }
                }
            }

            Vector3D localNegZAxis = matrix.Transform(new Vector3D(0, 0, -1));
            zAngle = Vector3D.AngleBetween(lookDirection, localNegZAxis);
            if (Round(zAngle, 5) != 0.0)
            {
                Vector3D normal = Vector3D.CrossProduct(lookDirection, localNegZAxis);
                matrix.Rotate(new Quaternion(normal, zAngle));
                if (Round(matrix.Transform(localNegZAxis).Z, 4) != Round(localNegZAxis.Z, 4))
                {
                    matrix.Rotate(new Quaternion(normal, zAngle * -2.0));
                }
            }

            matrix.Translate(new Vector3D(position.X, position.Y, position.Z));
            return matrix;
        }

        public static void LookAt(this ProjectionCamera camera, Point3D target, Vector3D lookDirection)
        {
            camera.Position = target - lookDirection;
        }

        public static void ZoomExtents(this ProjectionCamera camera, Rect3D bounds, double width, double height)
        {
            Vector3D diagonal = new Vector3D(bounds.SizeX, bounds.SizeY, bounds.SizeZ);
            Point3D center = bounds.Location + (diagonal * 0.5);
            double radius = diagonal.Length * 0.5;

            if (camera is PerspectiveCamera perspectiveCamera)
            {
                double disth = radius / Math.Tan(0.5 * perspectiveCamera.FieldOfView * Math.PI / 180);
                double vfov = perspectiveCamera.GetVerticalFieldOfView(width, height);

                double distv = radius / Math.Tan(0.5 * vfov * Math.PI / 180);
                double dist = Math.Max(disth, distv);
                Vector3D dir = perspectiveCamera.LookDirection;
                dir.Normalize();
                perspectiveCamera.LookAt(center, dir * dist);
            }
            if (camera is OrthographicCamera orthographicCamera)
            {
                double newWidth = radius * 2;
                if (width > height)
                {
                    newWidth = radius * 2 * width / height;
                }
                orthographicCamera.LookAt(center, orthographicCamera.LookDirection);
                orthographicCamera.Width = newWidth;
            }
        }

        public static void ZoomIn(this ProjectionCamera camera, Point3D center, double factor = 0.5) => Zoom(camera, center, factor);

        public static void ZoomOut(this ProjectionCamera camera, Point3D center, double factor = 2.0) => Zoom(camera, center, factor);

        public static void Zoom(this ProjectionCamera camera, Point3D center, double factor)
        {
            if (camera != null)
            {
                if (camera is PerspectiveCamera perspectiveCamera)
                {
                    Vector3D scaledLookDir = center - camera.Position;
                    scaledLookDir *= factor;
                    camera.LookAt(center, scaledLookDir);
                }
                if (camera is OrthographicCamera orthographicCamera)
                {
                    orthographicCamera.Width *= factor;
                }
            }
        }

        public static ProjectionCamera GetTransformedCamera(this ProjectionCamera camera, Transform3D transform)
        {
            if (camera is PerspectiveCamera perspectiveCamera)
            {
                return perspectiveCamera.GetTransformedPerspectiveCamera(transform);
            }

            if (camera is OrthographicCamera orthographicCamera)
            {
                return orthographicCamera.GetTransformedCamera(transform);
            }

            return camera;
        }
    }
}