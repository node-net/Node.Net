using System.Collections;
using System.Collections.Generic;
using System.Windows.Media.Media3D;

namespace Node.Net.Factories
{
    public sealed class Visual3DFactory : Generic.TargetTypeFactory<Visual3D>
    {
        public override Visual3D Create(object source)
        {
            if (source != null)
            {
                if (typeof(IDictionary).IsAssignableFrom(source.GetType()))
                {
                    var instance = CreateFromDictionary(source as IDictionary);
                    if (instance != null) return instance;
                }
            }

            if (Helper != null)
            {

            }
            return null;
        }

        public bool Cache
        {
            get { return cache; }
            set
            {
                if(cache != value)
                {
                    cache = value;
                    if (!cache) model3DCache.Clear();
                }
            }
        }
        private bool cache = false;
        private readonly Dictionary<IDictionary, Model3D> model3DCache = new Dictionary<IDictionary, Model3D>();
        private Visual3D CreateFromDictionary(IDictionary source)
        {
            
            if (Helper != null)
            {
                Model3D model = null;
                if (model3DCache.ContainsKey(source)) model = model3DCache[source];
                else
                {
                    model = Helper.Create(typeof(Model3D), source) as Model3D;
                    if (cache && model != null) model3DCache.Add(source, model);
                }
                if (model != null)
                {
                    return new ModelVisual3D { Content = model };
                }
            }
            return null;
        }
    }
}
