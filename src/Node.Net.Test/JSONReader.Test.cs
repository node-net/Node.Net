using NUnit.Framework;
using System;

namespace Node.Net.Test
{
	[TestFixture]
	internal class JSONReaderTest
	{
		[Test]
		public void MemoryCheck()
		{
			using (var reader = new Internal.JSONReader())
			{
				int z = 0;
			}
				
			GC.Collect();
			GC.WaitForPendingFinalizers();
			GC.WaitForFullGCComplete();
			GC.Collect();

			int x = 0;
			/*
			reader = new Internal.JSONReader();
			reader = null;
			GC.Collect();
			GC.WaitForPendingFinalizers();
			GC.WaitForFullGCComplete();
			GC.Collect();*/
			int y = 0;
		}
	}
}