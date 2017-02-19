using System.Windows.Media.Media3D;

namespace Node.Net.Deprecated.Factories
{
    public sealed class MeshGeometry3DFactory : Generic.TargetTypeFactory<MeshGeometry3D>
    {
        public override MeshGeometry3D Create(object source)
        {
            /*
            if (source != null)
            {
                MeshGeometry3D result = null;
                if (source.GetType() == typeof(string))
                {
                    result = CreateFromString(source.ToString());
                    if (result != null) return result;
                }

            }

            if (Helper != null)
            {

            }*/

            return null;
        }

        private MeshGeometry3D CreateFromString(string name)
        {
            //if (ContainsKey(name)) return this[name];
            return null;
        }
    }
}
