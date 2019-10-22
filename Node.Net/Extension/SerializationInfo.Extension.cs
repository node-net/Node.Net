using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Node.Net
{
	public static class SerializationInfoExtension
	{
		public static IDictionary GetPersistentData(this SerializationInfo info)
		{
			var data = new Dictionary<string, object>();
			SerializationInfoEnumerator e = info.GetEnumerator();
			while (e.MoveNext())
			{
				data.Add(e.Name, e.Value);
			}
			return data;
		}

		public static void SetPersistentData(this SerializationInfo info, IDictionary data)
		{
			foreach (string key in data.Keys)
			{
				info.AddValue(key, data[key]);
			}
		}
	}
}