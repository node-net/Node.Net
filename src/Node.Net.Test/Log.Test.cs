using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Node.Net.Test
{
	[TestFixture]
	class LogTest
	{
		[Test]
		public void Usage()
		{
			Log.Info("info");
			Log.Debug("debug");
			Log.Error("error");
			Log.Fatal("fatal");
			Log.Warn("warn");
		}
	}
}
