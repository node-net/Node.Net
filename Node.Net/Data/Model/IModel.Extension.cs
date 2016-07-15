using System;
using System.Collections;
using System.IO;

namespace Node.Net.Data.Model
{
    public static class IModelExtension
    {
        public static IModel Clone(IModel imodel)
        {
            if (imodel != null)
            {
                using (MemoryStream memory = new MemoryStream())
                {
                    var writer = new Writers.JsonWriter();
                    writer.Write(memory, imodel);
                    var reader = new Readers.Reader(imodel.GetType().Assembly);
                    memory.Seek(0, SeekOrigin.Begin);
                    return reader.Read(memory) as IModel;
                }
            }
            return null;
        }

        public static int ComputeHashCode(IModel model)
        {
            return _ComputeHashCode(model);
        }
        private static int _ComputeHashCode(object value)
        {
            if (!object.ReferenceEquals(null, value))
            {
                if (value.GetType() == typeof(bool) ||
                   value.GetType() == typeof(double) ||
                    value.GetType() == typeof(string)) return value.GetHashCode();
                else
                {
                    if (typeof(IDictionary).IsAssignableFrom(value.GetType())) return _ComputeHashCode(value as IDictionary);
                    if (typeof(IEnumerable).IsAssignableFrom(value.GetType())) return _ComputeHashCode(value as IEnumerable);
                }
            }
            return 0;
        }
        private static int _ComputeHashCode(IDictionary value)
        {
            var hashCode = value.Count;
            foreach (string key in value.Keys)
            {
                hashCode = hashCode ^ _ComputeHashCode(key) ^ _ComputeHashCode(value[key]);
            }
            return hashCode;
        }

        private static int _ComputeHashCode(IEnumerable value)
        {
            var count = 0;
            var hashCode = 0;
            foreach (object item in value)
            {
                var tmp = _ComputeHashCode(item);
                if (tmp != 0) count++;
                hashCode = hashCode ^ tmp;
            }
            hashCode = hashCode ^ count;
            return hashCode;
        }
        
        public static T Get<T>(IModel dictionary, string name)
        {
            if (!dictionary.Contains(name)) return default(T);
            var value = dictionary[name];
            if (value == null) return default(T);

            if (typeof(T) == typeof(DateTime) && value.GetType() == typeof(string))
            {
                return (T)((object)DateTime.Parse(value.ToString()));
            }
            return (T)dictionary[name];

        }

        public static void Set(IModel dictionary, string key, object value)
        {
            if (value != null && value.GetType() == typeof(DateTime))
            {
                dictionary[key] = ((DateTime)value).ToString("o");
            }
            else { dictionary[key] = value; }

            if(!dictionary.Contains(nameof(Type)))
            {
                SetTypeName(dictionary);
            }
        }

        public static void SetTypeName(IModel dictionary)
        {
            dictionary[nameof(Type)] = dictionary.GetType().Name;
        }
    }
}
