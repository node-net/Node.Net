using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Node.Net
{
    public sealed class MetaData
    {
        public static MetaData Default { get; } = new MetaData();

        public MetaData()
        {
            //data = new Dictionary<WeakReference, IDictionary>(new WeakReferenceComparer());
        }

        public bool HasMetaData(object item)
        {
            return data.ContainsKey(new WeakReference(item));
        }

        public IDictionary GetMetaData(object item)
        {
            if (item is null)
            {
                return new Dictionary<string, object>();
            }

            if (data.ContainsKey(new WeakReference(item)))
            {
                return data[new WeakReference(item)];
            }
            var metaData = new Dictionary<string, dynamic>();
            data.Add(new WeakReference(item), metaData);
            return metaData;
        }

        public void SetMetaData(object item, string name, object value)
        {
            GetMetaData(item)[name] = value;
        }

        public object? GetMetaData(object item, string name)
        {
            if (item == null)
            {
                return null;
            }

            var metaData = GetMetaData(item);
            if (metaData.Contains(name))
            {
                return metaData[name];
            }

            return null;
        }

        public T GetMetaData<T>(object item, string name)
        {
            var instance = GetMetaData(item, name);
            if (instance != null && (instance is T))
            {
                return (T)instance;
            }
            if (typeof(T) == typeof(string))
            {
                return (T)(object)"";
            }

            return default!;
        }

        public void ClearMetaData(object item)
        {
            if (item != null)
            {
                var wr = new WeakReference(item);
                data.Remove(wr);
            }
            Clean();
        }

        public void Clean()
        {
            var deadKeys = new List<WeakReference>();
            foreach (var wr in data.Keys)
            {
                if (!wr.IsAlive)
                {
                    deadKeys.Add(wr);
                }
            }
            foreach (var deadKey in deadKeys)
            {
                data.Remove(deadKey);
            }
        }

        private readonly Dictionary<WeakReference, IDictionary> data
            = new Dictionary<WeakReference, IDictionary>(new WeakReferenceComparer());
    }

    internal class WeakReferenceComparer : IEqualityComparer<WeakReference>
    {
        public bool Equals(WeakReference x, WeakReference y)
        {
            /*
            if (x is null)
            {
                return y is null;
            }

            if (y is null)
            {
                return false;
            }

            if (x.Target is null && y.Target is null)
            {
                return true;
            }*/

            if (x.Target is null)
            {
                return false;
            }

            return x.Target.Equals(y.Target);
        }

        public int GetHashCode(WeakReference obj)
        {
            if (obj.Target == null)
            {
                return 0;
            }

            return obj.Target.GetHashCode();
        }
    }
}
