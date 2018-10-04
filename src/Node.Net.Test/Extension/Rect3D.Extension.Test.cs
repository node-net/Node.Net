using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using NUnit.Framework;

namespace Node.Net.Test.Extension
{
	[TestFixture]
	class Rect3DExtensionTest
	{
		[Test]
		public void Usage()
		{
			var rect = new Rect3D();
			rect.GetCenter();
			rect.Scale(1.1);
		}
	}
}
