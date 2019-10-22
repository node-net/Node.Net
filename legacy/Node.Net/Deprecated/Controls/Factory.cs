using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace Node.Net.Deprecated.Controls
{
    public class Factory : IFactory
    {
        private static Factory _default;
        public static Factory Default
        {
            get
            {
                if (_default == null)
                {
                    _default = new Factory();
                }
                return _default;
            }
        }

        public Dictionary<Type, Type> AbstractTypeMap = new Dictionary<Type, Type>();
        public Dictionary<Type, IFactory> TypeFactories = new Dictionary<Type, IFactory>();

        public T Create<T>(object value)
        {
            if (value != null && typeof(Stream).IsAssignableFrom(value.GetType())) return CreateFromStream<T>(value as Stream);
            if (AbstractTypeMap.ContainsKey(typeof(T)))
            {
                var result = (T)Activator.CreateInstance(AbstractTypeMap[typeof(T)]);
                var frameworkElement = result as FrameworkElement;
                if (frameworkElement != null) frameworkElement.DataContext = value;
                return result;
            }
            if (TypeFactories.ContainsKey(typeof(T))) return TypeFactories[typeof(T)].Create<T>(value);
            if (!typeof(T).IsAbstract)
            {
                return Activator.CreateInstance<T>();
            }
            return default(T);
        }

        private T CreateFromStream<T>(Stream stream)
        {
            return (T)Internal.ImageSourceReader.Default.Read(stream);
        }
    }
}
