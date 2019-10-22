using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Node.Net.Test.Extension
{
	[TestFixture]
	internal static class ActionExtensionTest
	{
		[Test]
		public static void Invoke()
		{
			Action<int> a1 = Invoke1;
			a1.Invoke(new object[] { 5 });

			Action<int, int> a2 = Invoke2;
			a2.Invoke(new object[] { 5, 6 });
		}

		private static void Invoke1(int a) { }
		private static void Invoke2(int a,int b) { }
	}
}
