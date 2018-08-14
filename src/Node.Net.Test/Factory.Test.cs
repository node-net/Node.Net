using NUnit.Framework;
using System.Windows.Media.Media3D;

namespace Node.Net.Test
{
	[TestFixture]
	internal class FactoryTest
	{
		[Test]
		public void Create()
		{
			//Assert.NotNull(Factory.Default, "Factory.Default");
			var matrix = new Factory().Create<Matrix3D>();
		}
	}
}