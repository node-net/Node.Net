using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Node.Net.Beta.Internal.Factories
{
    sealed class Model3DFactory : IFactory
    {
        public object Create(Type target_type, object source)
        {
            if (source != null)
            {
                if (typeof(IDictionary).IsAssignableFrom(source.GetType())) return CreateFromDictionary(source as IDictionary);
            }
            if (ParentFactory != null)
            {
                var dictionary = ParentFactory.Create<IDictionary>(source);
                if (dictionary != null) return Create(typeof(IDictionary), dictionary);
            }
            return null;
        }
        public IFactory ParentFactory { get; set; }
        public Func<IDictionary, Model3D> PrimaryModel3DHelperFunction { get; set; }

        private bool cache = false;
        private Dictionary<object, Model3D> model3DCache = new Dictionary<object, Model3D>();
        public bool Cache
        {
            get { return cache; }
            set
            {
                if (cache != value)
                {
                    cache = value;
                    model3DCache.Clear();
                }
            }
        }
        private Model3D CreateFromDictionary(IDictionary source)
        {
            if (cache)
            {
                if (model3DCache.ContainsKey(source)) return model3DCache[source];
            }
            var model3DGroup = new Model3DGroup { Transform = GetTransform3D(source) };
            var primaryModel = GetPrimaryModel3D(source);
            if (primaryModel != null) model3DGroup.Children.Add(primaryModel);

            foreach (var key in source.Keys)
            {
                var child_dictionary = source[key] as IDictionary;
                if (child_dictionary != null)
                {
                    var child_model = CreateFromDictionary(child_dictionary);
                    if (child_model != null) model3DGroup.Children.Add(child_model);
                }
            }
            if (model3DGroup.Children.Count > 0)
            {
                if (cache) model3DCache.Add(source, model3DGroup);
                return model3DGroup;
            }
            return null;
        }

        private Model3D GetPrimaryModel3D(IDictionary source)
        {
            if (PrimaryModel3DHelperFunction != null)
            {
                var model = PrimaryModel3DHelperFunction(source);
                if (model != null) return model;
            }
            if (ParentFactory != null)
            {
                var geometry3D = ParentFactory.Create(typeof(GeometryModel3D), source) as GeometryModel3D;
                if (geometry3D != null) return geometry3D;
            }
            return null;
        }
        private Transform3D GetTransform3D(object source)
        {
            if (ParentFactory != null)
            {
                return ParentFactory.Create(typeof(Transform3D), source) as Transform3D;
            }
            return null;
        }

    }
}
