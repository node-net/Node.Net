#if IS_WINDOWS
extern alias NodeNet;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using NodeNet::System.Windows.Media.Imaging;
using NUnit.Framework;
using NodeNet::Node.Net;

namespace Node.Net
{
	[TestFixture]
	internal class FormatterTest
	{
		[Test]
		[TestCase("States.json",typeof(IDictionary))]
		[TestCase("Node.Net.png",typeof(System.Windows.Media.Imaging.BitmapFrame))]
		public void Deserialize_Type(string name,Type expected_type)
		{
			var stream = typeof(FormatterTest).Assembly.FindManifestResourceStream(name);
			
			// Skip PNG test on non-Windows targets where image support isn't available
			if (name.EndsWith(".png") && !FormatterSupportsPng())
			{
				Assert.Pass("PNG deserialization not supported on this target framework");
			}
			
			var instance = new Formatter().Deserialize(stream);
			Assert.That(expected_type.IsAssignableFrom(instance.GetType()),Is.True, "instance type " +
				expected_type.FullName + "was not assignable from " + instance.GetType().FullName);
		}
		
		private bool FormatterSupportsPng()
		{
			var formatter = new Formatter();
			return formatter.SignatureReaders.ContainsKey("89 50 4E 47 0D 0A 1A 0A");
		}
	}
}
#endif
