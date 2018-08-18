using NUnit.Framework;
using System;
using System.IO;
using System.Windows.Media;

namespace Node.Net.Test
{
	[TestFixture]
	internal class ReaderTest
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

		[Test]
		public void MemoryCheck()
		{
			var reader = new Reader();
			reader = null;
			GC.Collect();
			GC.WaitForPendingFinalizers();
			GC.WaitForFullGCComplete();
			GC.Collect();

			int x = 0;

			reader = new Reader();
			reader = null;
			GC.Collect();
			GC.WaitForPendingFinalizers();
			GC.WaitForFullGCComplete();
			GC.Collect();
			int y = 0;

			reader = new Reader();
			var imageStream = typeof(ReaderTest).Assembly.GetManifestResourceStream
				("Node.Net.Test.Resources.Node.Net.256.png");
			var image = reader.Read<ImageSource>(imageStream);
			Assert.NotNull(image);
			image = null;
			GC.Collect();
			GC.WaitForPendingFinalizers();
			GC.WaitForFullGCComplete();
			GC.Collect();
			int z = 0;
		}
	}
}