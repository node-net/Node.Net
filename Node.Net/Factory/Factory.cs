using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Node.Net.Factory
{
    public class Factory : IFactory
    {
        public static Factory Default { get; set; } = new Factory();
        public Factory()
        {
            ValueTypeFactories.Add(typeof(Stream), new Internal.ValueStreamFactory());
            ValueTypeFactories.Add(typeof(String), valueStringFactory);
            TypeFactories.Add(typeof(Color), new Internal.ColorFactory());
            TypeFactories.Add(typeof(ITypeName), new Internal.ITypeNameFactory());
            TypeFactories.Add(typeof(ILength), new Internal.ILengthFactory());
            TypeFactories.Add(typeof(IAngle), new Internal.IAngleFactory());
            TypeFactories.Add(typeof(ITranslation), new Internal.ITranslationFactory());
            TypeFactories.Add(typeof(Matrix3D), new Internal.Matrix3DFactory());
            TypeFactories.Add(typeof(Transform3D), new Internal.Transform3DFactory());
            TypeFactories.Add(typeof(GeometryModel3D), new Internal.GeometryModel3DFactory());
            TypeFactories.Add(typeof(Visual3D), new Internal.Visual3DFactory());
        }

        public List<Assembly> ResourceAssemblies = new List<Assembly>();
        private readonly Internal.ValueStringFactory valueStringFactory = new Internal.ValueStringFactory();
        public Dictionary<Type, IFactory> ValueTypeFactories = new Dictionary<Type, IFactory>();
        public Dictionary<Type, IFactory> TypeFactories = new Dictionary<Type, IFactory>();
        public object Create(Type targetType, object value)
        {
            valueStringFactory.ResourceFactory.ResourceAssemblies = ResourceAssemblies;
            if (value != null)
            {
                foreach (var type in ValueTypeFactories.Keys)
                {
                    if (type.IsAssignableFrom(value.GetType()))
                    {
                        var result = ValueTypeFactories[type].Create(targetType, value);
                        if (result != null)
                        {
                            if (targetType.IsAssignableFrom(result.GetType())) return result;
                            if (!type.IsAssignableFrom(result.GetType())) return Create(targetType, result);
                        }
                    }
                }
            }
            foreach (var type in TypeFactories.Keys)
            {
                if (type.IsAssignableFrom(targetType)) return TypeFactories[type].Create(targetType, value);
            }
            if (value == null) return null;

            return null;
        }
    }
}
