using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using NUnit.Framework;

namespace Node.Net.Test
{
	[TestFixture,Category("BitmapSourceWriter")]
	class BitmapSourceWriterTest
	{
		[Test]
		public void Write()
		{
			var image = Reader.Default.Read("image.bmp") as ImageSource;
			Assert.NotNull(image, nameof(image));

			var writer = new BitmapSourceWriter();
			using (var memory = new MemoryStream())
			{
				writer.Write(memory, image);
			}
			using (var memory2 = new MemoryStream())
			{
				writer.BitmapEncoder = new PngBitmapEncoder();
				writer.Write(memory2, image);
			}
			writer.Write(null, null);
			writer.Write(new MemoryStream(), image);
			writer.Write(new MemoryStream(), DateTime.Now);
			writer.BitmapEncoder = null;
			writer.Write(new MemoryStream(), image);
		}
	}
}
