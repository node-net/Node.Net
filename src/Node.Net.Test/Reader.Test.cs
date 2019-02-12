using NUnit.Framework;
using System.Collections;
using System.IO;

namespace Node.Net.Test
{
	[TestFixture]
	internal class ReaderTest
	{
		[TestCase("Object.Coverage.json")]
		public void Read(string name)
		{
			var reader = new Reader();
			var assembly = typeof(ReaderTest).Assembly;
			var stream = assembly.FindManifestResourceStream(name);
			Assert.NotNull(stream, nameof(stream));
			using (var memory = new MemoryStream())
			{
				stream.CopyTo(memory);
				memory.Seek(0, SeekOrigin.Begin);

				var i = reader.Read<IDictionary>(memory);
				Assert.NotNull(i, nameof(i));
				Assert.True(i.Contains("string_symbol"), "i.Contains 'string_symbol'");
				Assert.AreEqual("0°", i["string_symbol"].ToString(), "i['string_symbol']");

				memory.Seek(0, SeekOrigin.Begin);
				var filename = Path.GetTempFileName();
				using (var fs = new FileStream(filename, FileMode.Create))
				{
					memory.CopyTo(fs);
				}
				var d = reader.Read(filename) as IDictionary;
				Assert.NotNull(d, nameof(d));
				Assert.True(d.Contains("string_symbol"), "d.Contains 'string_symbol'");
				Assert.AreEqual("0°", d["string_symbol"].ToString(), "d['string_symbol']");

				using (var memory2 = new MemoryStream())
				{
					d.Save(memory);
				}
			}
		}
	}
}