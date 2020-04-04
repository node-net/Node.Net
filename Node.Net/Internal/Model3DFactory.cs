using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Media.Media3D;

namespace Node.Net.Internal
{
    internal sealed class Model3DFactory : IFactory
    {
        public object? Create(Type targetType, object source)
        {
            if (targetType != typeof(Model3D))
            {
                return null;
            }

            if (Ignore(source))
            {
                return null;
            }
            if (source != null)
            {
                if (source is IDictionary dictionary)
                {
                    return CreateFromDictionary(dictionary);
                }
            }
            if (ParentFactory != null)
            {
                IDictionary? dictionary = ParentFactory.Create<IDictionary>(source);
                if (dictionary != null)
                {
                    return CreateFromDictionary(dictionary);
                }
            }
            return null;
        }

        public List<Type> IgnoreTypes { get; } = new List<Type>();

        private bool Ignore(object source)
        {
            Type? sourceType = source.GetType();
            if (IgnoreTypes.Contains(sourceType))
            {
                return true;
            }

            foreach (Type? ignoreType in IgnoreTypes)
            {
                if (ignoreType.IsAssignableFrom(sourceType))
                {
                    return true;
                }
            }
            return false;
        }

        public IFactory ParentFactory { get; set; }
        public Func<IDictionary, Model3D> PrimaryModel3DHelperFunction { get; set; }
        public bool ScalePrimaryModel { get; set; } = true;
        private bool cache = true;

        public Dictionary<object, Model3D> Model3DCache { get; set; } = new Dictionary<object, Model3D>();

        public bool Cache
        {
            get { return cache; }
            set
            {
                if (cache != value)
                {
                    cache = value;
                    Model3DCache.Clear();
                }
            }
        }

        public void ClearCache()
        {
            Model3DCache.Clear(); namedCache.Clear();
        }

        public void ClearCache(object model)
        {
            if (model != null) { Model3DCache.Remove(model); }
        }

        private Model3D CreateFromDictionary(IDictionary source)
        {
            if (source == null)
            {
#pragma warning disable CS8603 // Possible null reference return.
                return null;
#pragma warning restore CS8603 // Possible null reference return.
            }

            if (cache && Model3DCache.ContainsKey(source))
            {
                return Model3DCache[source];
            }
            Model3DGroup? model3DGroup = new Model3DGroup { Transform = GetTransform3D(source) };
            Model3D? primaryModel = GetPrimaryModel3D(source);
            if (primaryModel != null)
            {
                model3DGroup.Children.Add(primaryModel);
            }

            foreach (object? key in source.Keys)
            {
                if (source[key] is IDictionary child_dictionary && !Ignore(child_dictionary))
                {
                    //var child_model = Create(typeof(Model3D), child_dictionary) as Model3D;
                    Model3D? child_model = CreateFromDictionary(child_dictionary);
                    if (child_model != null)
                    {
                        model3DGroup.Children.Add(child_model);
                    }
                }
            }
            if (model3DGroup.Children.Count > 0)
            {
                if (cache)
                {
                    Model3DCache.Add(source, model3DGroup);
                }

                return model3DGroup;
            }
#pragma warning disable CS8603 // Possible null reference return.
            return null;
#pragma warning restore CS8603 // Possible null reference return.
        }

        private Model3D GetPrimaryModel3D(IDictionary source)
        {
            Model3D? model3D = GetUnscaledPrimaryModel3D(source);
            if (model3D != null && ScalePrimaryModel)
            {
                Transform3D? scaleTransform = GetScalingTransform(source);
                if (scaleTransform != null)
                {
                    Model3DGroup? scaledModel3D = new Model3DGroup { Transform = scaleTransform };
                    if (scaledModel3D != null)
                    {
                        scaledModel3D.Children.Add(model3D);
                        return scaledModel3D;
                    }
                }
            }
#pragma warning disable CS8603 // Possible null reference return.
            return model3D;
#pragma warning restore CS8603 // Possible null reference return.
        }

        private readonly Dictionary<string, Model3D> namedCache = new Dictionary<string, Model3D>();

        private Model3D GetUnscaledPrimaryModel3D(IDictionary source)
        {
            if (PrimaryModel3DHelperFunction != null)
            {
                Model3D? model = PrimaryModel3DHelperFunction(source);
                if (model != null)
                {
                    return model;
                }
            }
            if (ParentFactory != null)
            {
                string? type = source.Get<string>("Type");
                if (type.Length > 0)
                {
                    string? modelName = $"Model3D.{type}.xaml";
                    if (namedCache.ContainsKey(modelName))
                    {
                        Model3D? m3d = namedCache[modelName];
                        if (m3d != null)
                        {
                            return m3d;
                        }
                    }
                    else
                    {
                        Model3D? m3d = ParentFactory.Create<Model3D>(modelName);
                        namedCache.Add(modelName, m3d);
                        if (m3d != null)
                        {
                            return m3d;
                        }
                    }
                }

                if (ParentFactory.Create(typeof(GeometryModel3D), source) is GeometryModel3D geometry3D)
                {
                    return geometry3D;
                }
            }
#pragma warning disable CS8603 // Possible null reference return.
            return null;
#pragma warning restore CS8603 // Possible null reference return.
        }

        private Transform3D GetTransform3D(object source)
        {
            if (ParentFactory != null)
            {
#pragma warning disable CS8603 // Possible null reference return.
                return ParentFactory.Create(typeof(Transform3D), source) as Transform3D;
#pragma warning restore CS8603 // Possible null reference return.
            }
#pragma warning disable CS8603 // Possible null reference return.
            return null;
#pragma warning restore CS8603 // Possible null reference return.
        }

        public static Transform3D GetScalingTransform(IDictionary source)
        {
            if (source == null)
            {
                return new MatrixTransform3D();
            }

            double scaleX = 1.0;
            double scaleY = 1.0;
            double scaleZ = 1.0;

            double tmp = source.GetLengthMeters("Height");
            if (tmp != 0.0)
            {
                scaleZ = tmp;
            }

            tmp = source.GetLengthMeters("Width");
            if (tmp != 0.0)
            {
                scaleY = tmp;
            }

            tmp = source.GetLengthMeters("Length");
            if (tmp != 0.0)
            {
                scaleX = tmp;
            }

            if (scaleX != 1.0 || scaleY != 1.0 || scaleZ != 1.0)
            {
                Matrix3D matrix3D = new Matrix3D();
                matrix3D.Scale(new Vector3D(scaleX, scaleY, scaleZ));
                if (!matrix3D.IsIdentity)
                {
                    return new MatrixTransform3D { Matrix = matrix3D };
                }
            }
#pragma warning disable CS8603 // Possible null reference return.
            return null;
#pragma warning restore CS8603 // Possible null reference return.
        }
    }
}