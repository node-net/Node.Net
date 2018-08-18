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
	class WordReaderTest
	{
		[Test]
		public void Memory()
		{
			var streamReader = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes("a")));
			using (var wordReader = new WordReader(streamReader))
			{
				int x = 0;
			}

			GC.Collect();
			GC.WaitForPendingFinalizers();
			GC.WaitForFullGCComplete();
			GC.Collect();
			int y = 0;
		}
	}
}
