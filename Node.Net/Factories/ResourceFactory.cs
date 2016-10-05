using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace Node.Net.Factories
{
    public sealed class ResourceFactory : IFactory
    {
        public object Create(Type targetType, object source,IFactory helper)
        {
            if (source != null)
            {
                if (source.GetType() == typeof(string))
                {
                    var key = source.ToString();
                    if (cache.ContainsKey(key)) return cache[key];
                    if (Assembly != null)
                    {
                        var manifestResourceName = GetManifestResourceName(key);
                        if(manifestResourceName != null)
                        {
                            var value = ReadFunction(Assembly.GetManifestResourceStream(manifestResourceName));
                            if(value != null)
                            {
                                cache.Add(key, value);
                                return value;
                            }
                        }
                    }
                }
            }
            return null;
        }

        private string GetManifestResourceName(string key)
        {
            foreach (var manifestResourceName in Assembly.GetManifestResourceNames())
            {
                if (key == manifestResourceName) return manifestResourceName;
            }
            if(AllowPartialMatch)
            {
                foreach(var manifestResourceName in Assembly.GetManifestResourceNames())
                {
                    if (manifestResourceName.Contains(key)) return manifestResourceName;
                }
            }
            return null;
        }

        public bool AllowPartialMatch { get; set; } = true;
        public Func<Stream, object> ReadFunction { get; set; } = DefaultReadFunction;
        private Assembly assembly = null;
        public Assembly Assembly
        {
            get { return assembly; }
            set
            {
                if(assembly != value)
                {
                    assembly = value;
                    cache.Clear();
                }
            }
        }
        private Dictionary<string, object> cache = new Dictionary<string, object>();

        public static object DefaultReadFunction(Stream stream) { return XamlReader.Load(stream); }
    }
}
