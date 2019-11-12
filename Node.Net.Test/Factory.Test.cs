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
			var factory = new Factory();
			Assert.NotNull(factory.Create<Matrix3D>());
			//Assert.NotNull(factory.Create(typeof(Matrix3D),null));
		}

		[Test]
		public void ClearCache()
		{
			var factory = new Factory();
			factory.Cache = true;
			factory.ClearCache();
			if (factory.Cache) factory.Cache = false;

			var matrix = factory.Create<Matrix3D>();
			factory.ClearCache(matrix);
			factory.ClearCache();
		}

		[Test]
		public void Coverage()
		{
			var factory = new Factory();
		}
	}
}