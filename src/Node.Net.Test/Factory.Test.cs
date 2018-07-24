using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using NUnit.Framework;

namespace Node.Net.Test
{
	[TestFixture]
	class FactoryTest
	{
		[Test]
		public void Create()
		{
			Assert.NotNull(Factory.Default, "Factory.Default");
			var matrix = Factory.Default.Create<Matrix3D>();
		}
	}
}
