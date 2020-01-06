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
            Assert.True(command.CanExecute(null));

            command = new DelegateCommand(TestCommand, TestCanExecute);
            Assert.True(command.CanExecute(null));
            command.Execute(null);

            command = new DelegateCommand(null);
            command.Execute(null);
            command.CanExecuteChanged += Command_CanExecuteChanged;
        }

        private void Command_CanExecuteChanged(object sender, System.EventArgs e)
        {
            throw new System.NotImplementedException();
        }

        public bool TestCanExecute(object i) { return true; }

        public void TestCommand(object i)
        {
        }
    }
}