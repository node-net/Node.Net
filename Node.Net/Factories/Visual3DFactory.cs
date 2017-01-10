using System.Collections;
using System.Windows.Media.Media3D;

namespace Node.Net.Factories
{
    public sealed class Visual3DFactory : Generic.TargetTypeFactory<Visual3D>
    {
        public override Visual3D Create(object source)
        {
            return CreateFromDictionary(source as IDictionary);
        }

        private Visual3D CreateFromDictionary(IDictionary source)
        {
            if (source == null) return null;
            if (Helper != null)
            {
                var model = Helper.Create(typeof(Model3D), source) as Model3D;
                if (model != null)
                {
                    return new ModelVisual3D { Content = model };
                }
            }
            return null;
        }
    }
}
