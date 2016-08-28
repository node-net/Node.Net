using System.IO;
using System.Windows.Media.Media3D;

namespace Node.Net.Factory
{
    public static class Extensions
    {
        public static T Create<T>(this IFactory factory, object source) => Internal.IFactoryExtension.Create<T>(factory, source);
        public static object Create(this IFactory factory, object source) => Internal.IFactoryExtension.Create(factory, source);
        public static Point3D? HitTest(this Visual3D reference, Point3D point, Vector3D direction) => Extension.Visual3DExtension.HitTest(reference, point, direction);
        public static void SetName(this Stream stream, string name) => Extension.StreamExtension.SetName(stream, name);
        public static string GetName(this Stream stream) => Extension.StreamExtension.GetName(stream);

        public static T GetNearestAncestor<T>(this IChild child) => Internal.IChildExtension.GetNearestAncestor<T>(child);
        public static T GetFurthestAncestor<T>(this IChild child) => Internal.IChildExtension.GetFurthestAncestor<T>(child);
        public static IFactory GetRootAncestor(this IChild child) => Internal.IChildExtension.GetRootAncestor(child);
    }
}
