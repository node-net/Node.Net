using System;
using System.Collections;
using System.Collections.Generic;

namespace Node.Net.Beta.Internal.Collections
{
	internal class WeakReferenceComparer : IEqualityComparer<WeakReference>
	{
		public bool Equals(WeakReference x, WeakReference y)
		{
			if (x.Target == null)
			{
				if (y.Target == null) return true;
				return false;
			}
			return x.Target.Equals(y.Target);
		}

		public int GetHashCode(WeakReference obj)
		{
			return obj.Target.GetHashCode();
		}
	}

	internal sealed class MetaData
	{
		public static MetaData Default { get; } = new MetaData();

		public MetaData()
		{
			data = new Dictionary<WeakReference, IDictionary>(new WeakReferenceComparer());
		}

		public IDictionary GetMetaData(object item)
		{
			if (data.ContainsKey(new WeakReference(item)))
			{
				return data[new WeakReference(item)];
			}
			var metaData = new Dictionary<string, dynamic>();
			data.Add(new WeakReference(item), metaData);
			return metaData;
		}

		public object GetMetaData(object item, string name)
		{
			if (item == null) return null;
			var metaData = GetMetaData(item);
			if (metaData.Contains(name)) return metaData[name];
			return null;
		}

		public T GetMetaData<T>(object item, string name)
		{
			var instance = GetMetaData(item, name);
			if (instance != null && typeof(T).IsAssignableFrom(instance.GetType()))
			{
				return (T)instance;
			}
			if (typeof(T) == typeof(string)) return (T)(object)"";
			return default(T);
		}

		public void Clean()
		{
			var deadKeys = new List<WeakReference>();
			foreach (var wr in data.Keys)
			{
				if (!wr.IsAlive) deadKeys.Add(wr);
			}
			foreach (var deadKey in deadKeys) { data.Remove(deadKey); }
		}

		private readonly Dictionary<WeakReference, IDictionary> data = null;
	}
}