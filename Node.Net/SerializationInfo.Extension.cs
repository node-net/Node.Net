using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

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
	}
}
