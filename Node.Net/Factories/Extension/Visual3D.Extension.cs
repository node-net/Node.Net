using System.Windows.Media.Media3D;

namespace Node.Net.Factories.Extension
{
    static class Visual3DExtension
    {
        public static Point3D? HitTest(Visual3D reference, Point3D point, Vector3D direction)
        {
            var hitTester = new Helpers.HitTester();
            return hitTester.HitTest(reference, point, direction);
        }
    }
}
