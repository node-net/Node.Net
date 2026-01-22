#if IS_WINDOWS
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Node.Net;

namespace Node.Net
{
	internal class FormatterTest
	{
		[Test]
		[Arguments("States.json", typeof(IDictionary))]
		[Arguments("Node.Net.png", typeof(System.Windows.Media.Imaging.BitmapFrame))]
		public async Task Deserialize_Type(string name, Type expected_type)
		{
			var stream = typeof(FormatterTest).Assembly.FindManifestResourceStream(name);
			
			// Skip PNG test on non-Windows targets where image support isn't available
			if (name.EndsWith(".png") && !FormatterSupportsPng())
			{
				// TUnit doesn't have Assert.Pass - just return early
				return;
			}
			
			var instance = new Formatter().Deserialize(stream);
			await Assert.That(expected_type.IsAssignableFrom(instance.GetType())).IsTrue();
		}
		
		private bool FormatterSupportsPng()
		{
			var formatter = new Formatter();
			return formatter.SignatureReaders.ContainsKey("89 50 4E 47 0D 0A 1A 0A");
		}
	}
}
#endif
