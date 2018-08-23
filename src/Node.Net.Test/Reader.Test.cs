﻿using NUnit.Framework;
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
	}
}