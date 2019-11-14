using Node.Net.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Node.Net.Collections
{
	public class List : System.Collections.Generic.List<object>
	{
		public string ToJson()
		{
			return new JsonWriter().WriteToString(this);
		}

		public static List Parse(Stream stream)
		{
			return new JsonReader { DefaultArrayType = typeof(List) }.Read(stream) as List;
		}

		public static List Parse(string json)
		{
			return Parse(new MemoryStream(Encoding.UTF8.GetBytes(json)));
		}
	}
}
