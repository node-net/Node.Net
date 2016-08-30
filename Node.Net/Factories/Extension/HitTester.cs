using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Node.Net.Factories.Extension
{
    class HitTester
    {
        public Point3D? HitTest(Visual3D reference, Point3D point, Vector3D direction)
        {
            if (reference != null)
            {
                var hitParams = new RayHitTestParameters(point, direction);
                hit = false;
                VisualTreeHelper.HitTest(reference, null, HitTestCallback, hitParams);
                if (hit)
                {
                    return hitTestValue;
                }
            }
            return null;
        }
        private bool hit;
        private Point3D hitTestValue = new Point3D();
        private HitTestResultBehavior HitTestCallback(HitTestResult result)
        {
            var rayhit = result as RayHitTestResult;
            hitTestValue = rayhit.PointHit;
            hit = true;
            return HitTestResultBehavior.Continue;
        }
    }
}
