using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Node.Net.Factory
{
    public class Factory : IFactory
    {
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
            TypeFactories.Add(typeof(GeometryModel3D), new Internal.GeometryModel3DFactory());
            TypeFactories.Add(typeof(Visual3D), new Internal.Visual3DFactory());
        }


        private Internal.ValueStringFactory valueStringFactory = new Internal.ValueStringFactory();
        private IDictionary resources = new Dictionary<string, dynamic>();
        public IDictionary Resources
        {
            get
            {
                if (resources == null) resources = new Dictionary<string, dynamic>();
                return resources;
            }
            set
            {
                resources = value;
            }
        }

        public Dictionary<Type, IFactory> ValueTypeFactories = new Dictionary<Type, IFactory>();
        public Dictionary<Type, IFactory> TypeFactories = new Dictionary<Type, IFactory>();
        public object Create(Type targetType,object value)
        {
            valueStringFactory.ResourceFactory.ResourceAssemblies = ResourceAssemblies;
            if(value != null)
            {
                foreach(var type in ValueTypeFactories.Keys)
                {
                    if(type.IsAssignableFrom(value.GetType()))
                    {
                        var result = ValueTypeFactories[type].Create(targetType,value);
                        if(result != null)
                        {
                            if (targetType.IsAssignableFrom(result.GetType())) return result;
                            if (!type.IsAssignableFrom(result.GetType())) return Create(targetType, result);
                        }
                    }
                }
            }
            foreach (var type in TypeFactories.Keys)
            {
                if (type.IsAssignableFrom(targetType)) return TypeFactories[type].Create(targetType,value);
            }
            if (value == null) return null;
            // Exact match pass
            if (Resources.Contains(value))
            {
                var resourceValue = Resources[value];
                if (targetType.IsAssignableFrom(resourceValue.GetType()))
                {
                    return resourceValue;
                }
            }
            // partial match pass
            foreach(string key in Resources.Keys)
            {
                if(key.Contains(value.ToString()))
                {
                    var resourceValue = Resources[key];
                    if (targetType.IsAssignableFrom(resourceValue.GetType()))
                    {
                        return resourceValue;
                    }
                }
            }
            return null;
        }

        

        public static Factory Default { get; set; } = new Factory();
        public static List<Assembly> ResourceAssemblies = new List<Assembly>();


    }
}
