using System;
using System.Windows.Media.Media3D;

namespace Node.Net
{
    public static class ProjectionCameraExtension
    {
        public static Matrix3D GetWorldToLocal(this ProjectionCamera camera) => Beta.Internal.ProjectionCameraExtension.GetWorldToLocal(camera);
        public static Matrix3D GetLocalToWorld(this ProjectionCamera camera) => Beta.Internal.ProjectionCameraExtension.GetLocalToWorld(camera);
        public static void LookAt(this ProjectionCamera camera, Point3D target, Vector3D lookDirection)
        {
            camera.Position = target - lookDirection;
        }
        public static void ZoomExtents(this ProjectionCamera camera, Rect3D bounds, double width, double height)
        {
            var diagonal = new Vector3D(bounds.SizeX, bounds.SizeY, bounds.SizeZ);
            var center = bounds.Location + (diagonal * 0.5);
            double radius = diagonal.Length * 0.5;

            var perspectiveCamera = camera as PerspectiveCamera;
            if (perspectiveCamera != null)
            {
                double disth = radius / Math.Tan(0.5 * perspectiveCamera.FieldOfView * Math.PI / 180);
                double vfov = perspectiveCamera.GetVerticalFieldOfView(width, height);


                double distv = radius / Math.Tan(0.5 * vfov * Math.PI / 180);
                double dist = Math.Max(disth, distv);
                var dir = perspectiveCamera.LookDirection;
                dir.Normalize();
                perspectiveCamera.LookAt(center, dir * dist);
            }
            var orthographicCamera = camera as OrthographicCamera;
            if (orthographicCamera != null)
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
                var perspectiveCamera = camera as PerspectiveCamera;
                if (perspectiveCamera != null)
                {
                    var scaledLookDir = center - camera.Position;
                    scaledLookDir *= factor;
                    camera.LookAt(center, scaledLookDir);
                }
                var orthographicCamera = camera as OrthographicCamera;
                if (orthographicCamera != null)
                {
                    orthographicCamera.Width = orthographicCamera.Width * factor;
                }
            }
        }
        public static ProjectionCamera GetTransformedCamera(this ProjectionCamera camera, Transform3D transform)
        {
            var perspectiveCamera = camera as PerspectiveCamera;
            if (perspectiveCamera != null) return perspectiveCamera.GetTransformedPerspectiveCamera(transform);
            var orthographicCamera = camera as OrthographicCamera;
            if (orthographicCamera != null) return orthographicCamera.GetTransformedCamera(transform);
            return camera;
        }
    }
}
