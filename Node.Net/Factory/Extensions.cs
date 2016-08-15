using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Node.Net.Factory
{
    public static class Extensions
    {
        public static T Create<T>(this IFactory factory, object value) => Internal.IFactoryExtension.Create<T>(factory, value);
        public static Point3D? HitTest(this Visual3D reference, Point3D point, Vector3D direction) => Extension.Visual3DExtension.HitTest(reference, point, direction);
    }
}
