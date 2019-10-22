using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Node.Net.Test
{
	[TestFixture]
	class MetaDataTest
	{
		[Test]
		public void MetaData_Usage()
		{
			using (var i = new MemoryStream())
			{
				Assert.IsNull(MetaData.Default.GetMetaData(null, "Name"));
				Assert.False(MetaData.Default.HasMetaData(i));
				MetaData.Default.Clean();
				var md = MetaData.Default.GetMetaData(i, "Name");
				Assert.IsNull(md, nameof(md));
				MetaData.Default.SetMetaData(i, "Name", "test");
				Assert.True(MetaData.Default.HasMetaData(i));
				Assert.AreEqual("test", MetaData.Default.GetMetaData(i, "Name").ToString());
				Assert.AreEqual("test", MetaData.Default.GetMetaData<string>(i, "Name"));
				Assert.AreEqual("", MetaData.Default.GetMetaData<string>(i, "Description"));
				Assert.AreEqual(0, MetaData.Default.GetMetaData<int>(i, "Size"));
			}
			MetaData.Default.Clean();
		}
	}
}
