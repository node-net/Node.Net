using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Node.Net.Factories.Extension
{
    static class VisualExtension
    {
        public static Point3D? HitTest(Visual visual, double x, double y)
        {
            var result = VisualTreeHelper.HitTest(visual, new Point(x, y)) as RayMeshGeometry3DHitTestResult;
            return result?.PointHit;
        }
    }
}
