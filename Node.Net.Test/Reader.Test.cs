﻿using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

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
			using var memory = new MemoryStream();
			stream.CopyTo(memory);
			memory.Seek(0, SeekOrigin.Begin);

            var memory2 = new MemoryStream();
            memory.CopyTo(memory2);
            memory.Seek(0, SeekOrigin.Begin);

			var i = reader.Read<IDictionary>(memory);
			Assert.NotNull(i, nameof(i));
			Assert.True(i.Contains("string_symbol"), "i.Contains 'string_symbol'");
			Assert.AreEqual("0°", i["string_symbol"].ToString(), "i['string_symbol']");

			memory2.Seek(0, SeekOrigin.Begin);
			var filename = Path.GetTempFileName();
			using (var fs = new FileStream(filename, FileMode.Create))
			{
				memory2.CopyTo(fs);
			}
			var d = reader.Read(filename) as IDictionary;
			Assert.NotNull(d, nameof(d));
			Assert.True(d.Contains("string_symbol"), "d.Contains 'string_symbol'");
			Assert.AreEqual("0°", d["string_symbol"].ToString(), "d['string_symbol']");

			using var memory3 = new MemoryStream();
			d.Save(memory3);
		}

		[Test]
		public void PreserveBackslash()
		{
			// https://stackoverflow.com/questions/19176024/how-to-escape-special-characters-in-building-a-json-string
			var data = new Dictionary<string, object>
			{
				{"User",@"Domain\User" }
			};

			var json = data.ToJson();
			Assert.True(json.Contains(@"Domain\u005cUser"), "json contains 'Domain\u005cUser'");

			using (var memory = new MemoryStream(Encoding.UTF8.GetBytes(json)))
			{
				var d = new Reader().Read<IDictionary>(memory);
				Assert.True(d.Contains("User"));
				var user = d["User"].ToString();
				Assert.AreEqual(@"Domain\User", d["User"].ToString());
			}

			json = "{\"User\":\"Domain\\User\"}";
			using (var memory = new MemoryStream(Encoding.UTF8.GetBytes(json)))
			{
				var d = new Reader().Read<IDictionary>(memory);
				Assert.True(d.Contains("User"));
				var user = d["User"].ToString();
				Assert.AreEqual(@"Domain\User", d["User"].ToString());
			}
		}

		[Test]
		public void PreserveBackslash2()
		{
			const string json = "{ \"path\" : \"C:\\\\tmp\" }";
			var data = new Reader()
				.Read<IDictionary>(new MemoryStream(Encoding.UTF8.GetBytes(json)));
			Assert.True(data.Contains("path"), "data.Contains 'path'");
			var path = data["path"].ToString();
			Assert.True(path.Contains("\\"), "path contains \\");
			Assert.AreEqual("C:\\tmp", path);
		}
	}
}