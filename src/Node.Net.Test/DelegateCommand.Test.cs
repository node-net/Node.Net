using NUnit.Framework;

namespace Node.Net.Test
{
	[TestFixture]
	internal class DelegateCommandTest
	{
		[Test]
		public void Usage()
		{
			var command = new DelegateCommand(TestCommand);
			command.Execute(null);
		}

		public void TestCommand(object i)
		{
		}
	}
}