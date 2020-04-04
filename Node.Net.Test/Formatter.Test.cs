using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.Imaging;
using NUnit.Framework;

namespace Node.Net
{
	[TestFixture]
	internal class FormatterTest
	{
		[Test]
		[TestCase("States.json",typeof(IDictionary))]
		[TestCase("Node.Net.png",typeof(BitmapFrame))]
		public void Deserialize_Type(string name,Type expected_type)
		{
			var stream = typeof(FormatterTest).Assembly.FindManifestResourceStream(name);
			Assert.NotNull(stream, name);
			var instance = new Formatter().Deserialize(stream);
			Assert.NotNull(instance, "instance was null for " + name);
			Assert.True(expected_type.IsAssignableFrom(instance.GetType()), "instance type " +
				expected_type.FullName + "was not assignable from " + instance.GetType().FullName);
		}
	}
}
