using System;
using System.Collections;
using System.Collections.Generic;

namespace Node.Net.Internal
{
    /// <summary>
    /// MetaData key,value pairs for objects
    /// </summary>
    internal sealed class MetaData
    {
        /// <summary>
        /// Static Default instance of MetaData
        /// </summary>
        public static MetaData Default { get; } = new MetaData();

        /// <summary>
        /// Default Constructor
        /// </summary>
        public MetaData()
        {
            data = new Dictionary<WeakReference, IDictionary>(new WeakReferenceComparer());
        }

        /// <summary>
        /// Test if an object has been assigned any metadata
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
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
            Dictionary<string, dynamic>? metaData = new Dictionary<string, dynamic>();
            data.Add(new WeakReference(item), metaData);
            return metaData;
        }

        /// <summary>
        /// Set metadata as name value pair
        /// </summary>
        /// <param name="item"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void SetMetaData(object item, string name, object value)
        {
            GetMetaData(item)[name] = value;
        }

        /// <summary>
        /// Get the metadata
        /// </summary>
        /// <param name="item"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public object GetMetaData(object item, string name)
        {
            if (item == null)
            {
                return null;
            }

            IDictionary? metaData = GetMetaData(item);
            if (metaData.Contains(name))
            {
                return metaData[name];
            }

            return null;
        }

        /// <summary>
        /// Get the metadata cast to a specific Type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public T GetMetaData<T>(object item, string name)
        {
            object? instance = GetMetaData(item, name);
            if (instance != null && (instance is T))
            {
                return (T)instance;
            }
            if (typeof(T) == typeof(string))
            {
                return (T)(object)"";
            }

            return default;
        }

        public void ClearMetaData(object item)
        {
            if (item != null)
            {
                WeakReference? wr = new WeakReference(item);
                data.Remove(wr);
            }
            Clean();
        }

        /// <summary>
        /// Cleans weak references that are no longer alive
        /// </summary>
        public void Clean()
        {
            try
            {
                List<WeakReference>? deadKeys = new List<WeakReference>();
                foreach (WeakReference? wr in data.Keys)
                {
                    if (!wr.IsAlive)
                    {
                        deadKeys.Add(wr);
                    }
                }
                foreach (WeakReference? deadKey in deadKeys)
                {
                    data.Remove(deadKey);
                }
            }
            catch { }
        }

        private readonly Dictionary<WeakReference, IDictionary> data = null;
    }

    internal class WeakReferenceComparer : IEqualityComparer<WeakReference>
    {
        public bool Equals(WeakReference x, WeakReference y)
        {
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
            }

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