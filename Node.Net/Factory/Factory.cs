using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Node.Net.Factory
{
    public class Factory : IFactory
    {
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
        public Dictionary<Type, IFactory> TypeFactories = new Dictionary<Type, IFactory>();
        public T Create<T>(object value)
        {
            foreach(var type in TypeFactories.Keys)
            {
                if (type.IsAssignableFrom(typeof(T))) return TypeFactories[type].Create<T>(value);
            }
            if (value == null) return default(T);
            if(Resources.Contains(value))
            {
                var resourceValue = Resources[value];
                if(typeof(T).IsAssignableFrom(resourceValue.GetType()))
                {
                    return (T)(object)resourceValue;
                }
            }
            return default(T);
        }

        public Factory()
        {
            TypeFactories.Add(typeof(Color), new Internal.ColorFactory());
            TypeFactories.Add(typeof(ILength), new Internal.ILengthFactory());
            TypeFactories.Add(typeof(Matrix3D), new Internal.Matrix3DFactory());
        }

        public static Factory Default { get; set; } = new Factory();
    }
}
