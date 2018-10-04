using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Node.Net.Test
{
	[TestFixture]
	class DelegateCommandTest
	{
		[Test]
		public void Usage()
		{
			var command = new DelegateCommand(TestCommand);
			command.Execute(null);
		}
		public void TestCommand(object i) { }
	}
}
