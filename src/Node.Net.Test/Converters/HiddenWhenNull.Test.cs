using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Node.Net.Test.Converters
{
	[TestFixture]
	class HiddenWhenNullTest
	{
		[Test]
		public void Usage()
		{
			var c = new Node.Net.Converters.HiddenWhenNull();
			c.Convert(null, typeof(object), null, null);
			c.Convert(1, typeof(object), null, null);
			Assert.Throws<NotImplementedException>(() => c.ConvertBack(null, typeof(object), null, null));
		}
	}
}
