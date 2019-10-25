using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NUnit.Framework;

namespace Node.Net.Test
{
	[TestFixture]
	class JsonWriterTest
	{
		[Test]
		public void Write()
		{
			var stream = typeof(JsonReaderTest)
				.Assembly
				.GetManifestResourceStream("Node.Net.Test.Resources.Object.Coverage.json");

			var dictionary = new JsonReader().Read(stream) as IDictionary;
			Assert.NotNull(dictionary, nameof(dictionary));
			Assert.AreEqual(14, dictionary.Count, "dictionary.Count");

			using (var memory = new MemoryStream())
			{
				new JsonWriter().Write(memory,dictionary);
				memory.Seek(0, SeekOrigin.Begin);
				var dictionary2 = new JsonReader().Read(memory) as IDictionary;
				Assert.NotNull(dictionary2, nameof(dictionary2));
				Assert.AreEqual(14, dictionary2.Count, "dictionary2.Count");
			}
		}

		[Test]
		public void Write_Recursive()
		{
			var data = new Dictionary<string, object>
			{
				{"Name","test" }
			};
			data["Self"] = data;
			using (var memory = new MemoryStream())
			{
				new JsonWriter().Write(memory, data);
			}
		}
	}
}
