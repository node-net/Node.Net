using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Node.Net
{
    public static class Viewport3DExtension
    {
        public static void ZoomExtents(this Viewport3D viewport)
        {
            ZoomExtents(viewport, FindBounds(viewport));
        }
        public static void ZoomExtents(this Viewport3D viewport, Rect3D bounds)
        {
            if (viewport.Camera != null)
            {
                var diagonal = new Vector3D(bounds.SizeX, bounds.SizeY, bounds.SizeZ);
                if (bounds.IsEmpty || diagonal.LengthSquared < double.Epsilon)
                {
                    return;
                }
                var projectionCamera = viewport.Camera as ProjectionCamera;
                if (projectionCamera != null)
                {
                    projectionCamera.ZoomExtents(bounds, viewport.ActualWidth, viewport.ActualHeight);
                }
            }
        }
        public static void ZoomOut(this Viewport3D viewport)
        {
            var projectionCamera = viewport.Camera as ProjectionCamera;
            if (projectionCamera != null)
            {
                projectionCamera.ZoomOut(FindCenter(viewport));
            }
        }
        public static void ZoomIn(this Viewport3D viewport)
        {
            var projectionCamera = viewport.Camera as ProjectionCamera;
            if (projectionCamera != null)
            {
                projectionCamera.ZoomIn(FindCenter(viewport));
            }
        }

        public static Rect3D FindBounds(this Viewport3D viewport)
        {
            var bounds = Rect3D.Empty;
            foreach (var visual in viewport.Children)
            {
                bounds.Union(VisualTreeHelper.GetDescendantBounds(visual));
            }

            return bounds;
        }
        private static Point3D FindCenter(this Viewport3D viewport)
        {
            return FindBounds(viewport).GetCenter();
        }
    }
}
