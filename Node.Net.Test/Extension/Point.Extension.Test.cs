using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using NUnit.Framework;

namespace Node.Net.Tests
{
	[TestFixture]
	internal class PointExtensionTest
	{
		[Test]
		public void GetPointsAtInterval()
		{
			var outline = new Point[] { new Point(0, 0), new Point(10, 0), new Point(10, 10), new Point(0, 10) };
			outline = outline.Close();
			Assert.AreEqual(5, outline.Length, "outline.Length");
			Assert.AreEqual(40.0, outline.GetLength(), "outline.GetLength()");
			var points = outline.GetPointsAtInterval(5.0);
			Assert.AreEqual(9, points.Length, "points.Length");
		}
	}
}