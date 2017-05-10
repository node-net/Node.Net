using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Media.Media3D;

namespace Node.Net.Beta.Internal.Factories
{
    sealed class Model3DFactory : IFactory
    {
        public object Create(Type target_type, object source)
        {
            if (source == null) return null;
            if (target_type == null) return null;
            if (target_type != typeof(Model3D)) return null;
            if (Ignore(source)) return null;
            //if (IgnoreTypes.Contains(sourceType)) return null;
            /*
            foreach(var ignoreType in IgnoreTypes)
            {
                if (ignoreType.IsAssignableFrom(sourceType)) return null;
            }*/
            if (source != null)
            {
                var dictionary = source as IDictionary;
                if(dictionary != null)
                {
                    return CreateFromDictionary(dictionary);
                }
            }
            if (ParentFactory != null)
            {
                var dictionary = ParentFactory.Create<IDictionary>(source);
                if (dictionary != null) return CreateFromDictionary(dictionary);
            }
            return null;
        }

        public List<Type> IgnoreTypes { get; } = new List<Type>();

        private bool Ignore(object source)
        {
            var sourceType = source.GetType();
            if (IgnoreTypes.Contains(sourceType)) return true;
            foreach (var ignoreType in IgnoreTypes)
            {
                if (ignoreType.IsAssignableFrom(sourceType)) return true;
            }
            return false;
        }
        public IFactory ParentFactory { get; set; }
        public Func<IDictionary, Model3D> PrimaryModel3DHelperFunction { get; set; }
        public bool ScalePrimaryModel { get; set; } = true;
        private bool cache = true;
        public Dictionary<object,Model3D> Model3DCache
        {
            get { return model3DCache; }
            set { model3DCache = value; }
        }
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
        public void ClearCache() { model3DCache.Clear(); namedCache.Clear(); }
        private Model3D CreateFromDictionary(IDictionary source)
        {
            if (source == null) return null;
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
                    if (!Ignore(child_dictionary))
                    {
                        //var child_model = Create(typeof(Model3D), child_dictionary) as Model3D;
                        var child_model = CreateFromDictionary(child_dictionary);
                        if (child_model != null) model3DGroup.Children.Add(child_model);
                    }
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
            var model3D = GetUnscaledPrimaryModel3D(source);
            if (model3D != null && ScalePrimaryModel)
            {
                var scaleTransform = GetScalingTransform(source);
                if (scaleTransform != null)
                {
                    var scaledModel3D = new Model3DGroup { Transform = scaleTransform };
                    if (scaledModel3D != null)
                    {
                        scaledModel3D.Children.Add(model3D);
                        return scaledModel3D;
                    }
                }
            }
            return model3D;
        }

        private Dictionary<string, Model3D> namedCache = new Dictionary<string, Model3D>();
        private Model3D GetUnscaledPrimaryModel3D(IDictionary source)
        {
            if (PrimaryModel3DHelperFunction != null)
            {
                var model = PrimaryModel3DHelperFunction(source);
                if (model != null) return model;
            }
            if (ParentFactory != null)
            {
                var type = source.Get<string>("Type");
                if (type.Length > 0)
                {
                    var modelName = $"Model3D.{type}.xaml";
                    if (namedCache.ContainsKey(modelName))
                    {
                        var m3d = namedCache[modelName];
                        if (m3d != null) return m3d;
                    }
                    else
                    {
                        var m3d = ParentFactory.Create<Model3D>(modelName);
                        namedCache.Add(modelName, m3d);
                        if (m3d != null) return m3d;
                        /*
                        var model3D = ParentFactory.Create<Model3D>($"{type}.Model3D.");
                        if (model3D != null) return model3D;
                        if (!locked)
                        {
                            try
                            {
                                locked = true;
                                model3D = ParentFactory.Create<Model3D>($"Model3D.{type}.");
                                if (model3D != null) return model3D;
                            }
                            finally { locked = false; }
                        }*/
                    }
                }

                var geometry3D = ParentFactory.Create(typeof(GeometryModel3D), source) as GeometryModel3D;
                if (geometry3D != null) return geometry3D;
            }
            return null;
        }
        private bool locked = false;
        private Transform3D GetTransform3D(object source)
        {
            if (ParentFactory != null)
            {
                return ParentFactory.Create(typeof(Transform3D), source) as Transform3D;
            }
            return null;
        }
        public static Transform3D GetScalingTransform(IDictionary source)
        {
            var scaleX = 1.0;
            var scaleY = 1.0;
            var scaleZ = 1.0;

            var tmp = source.GetLengthMeters("Height");
            if (tmp != 0.0) scaleZ = tmp;
            tmp = source.GetLengthMeters("Width");
            if (tmp != 0.0) scaleY = tmp;
            tmp = source.GetLengthMeters("Length");
            if (tmp != 0.0) scaleX = tmp;
            if (scaleX != 1.0 || scaleY != 1.0 || scaleZ != 1.0)
            {
                var matrix3D = new Matrix3D();
                matrix3D.Scale(new Vector3D(scaleX, scaleY, scaleZ));
                if (!matrix3D.IsIdentity) return new MatrixTransform3D { Matrix = matrix3D };
            }
            return null;
        }
    }
}
