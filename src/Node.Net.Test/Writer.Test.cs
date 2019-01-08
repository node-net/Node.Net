using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Node.Net.Test
{
	[TestFixture]
	internal static class WriterTest
	{
		[Test]
		public static void WriteJson()
		{
			var data = new Dictionary<string, object>
			{
				{"name","test" }
			};

			var writer = new Writer { JsonFormat = JsonFormat.Compact };
			using (var memory = new MemoryStream())
			{
				writer.Write(memory, data);
				var json = Encoding.UTF8.GetString(memory.ToArray());
				Assert.AreEqual("{\"name\":\"test\"}", json);
			}

			writer = new Writer { JsonFormat = JsonFormat.Pretty };
			using (var memory = new MemoryStream())
			{
				writer.Write(memory, data);
				var json = Encoding.UTF8.GetString(memory.ToArray());
				Assert.AreEqual("{\r\n  \"name\":\"test\"\r\n}", json);
			}
		}
	}
}