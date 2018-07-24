using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Node.Net;
using NUnit.Framework;

namespace Node.Net.Test
{
	[TestFixture]
	class ReaderTest
	{
		//[Test]
		//[TestCase("Object.Coverage.json")]
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

				var i = reader.Read(memory);
				Assert.NotNull(i, nameof(i));

				memory.Seek(0, SeekOrigin.Begin);
				var filename = Path.GetTempFileName();
				using (var fs = new FileStream(filename, FileMode.Create))
				{
					memory.CopyTo(fs);
				}
				i = reader.Read(filename);
				Assert.NotNull(i, nameof(i));
			}
		}
	}
}
