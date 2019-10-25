using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Node.Net
{
	[TestFixture]
	class ElementTest
	{
		[Test]
		public void Usage()
		{
			var states_stream = Sample.Files.Repository.GetStream("Json/States.json");
			Assert.NotNull(states_stream, nameof(states_stream));
			var element = Element.Open(states_stream);
		}
	}
}
