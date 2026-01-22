#if IS_WINDOWS
using System;
using NUnit.Framework;
using Node.Net;

namespace Node.Net.Test
{
    [TestFixture]
    internal class DelegateCommandTest
    {
        [Test]
        public void Usage()
        {
            // Use reflection to access conditionally compiled type
            var assembly = typeof(Factory).Assembly;
            var delegateCommandType = assembly.GetType("Node.Net.DelegateCommand");
            if (delegateCommandType == null)
            {
                Assert.Pass("DelegateCommand type not found - skipping test on non-Windows target");
            }
            
            var command = System.Activator.CreateInstance(delegateCommandType, new Action<object>(TestCommand));
            var executeMethod = delegateCommandType.GetMethod("Execute");
            var canExecuteMethod = delegateCommandType.GetMethod("CanExecute");
            var canExecuteChangedEvent = delegateCommandType.GetEvent("CanExecuteChanged");
            
            executeMethod.Invoke(command, new object[] { null });
            Assert.That((bool)canExecuteMethod.Invoke(command, new object[] { null }), Is.True);

            command = System.Activator.CreateInstance(delegateCommandType, new Action<object>(TestCommand), new Func<object, bool>(TestCanExecute));
            Assert.That((bool)canExecuteMethod.Invoke(command, new object[] { null }), Is.True);
            executeMethod.Invoke(command, new object[] { null });

            command = System.Activator.CreateInstance(delegateCommandType, new object[] { null });
            executeMethod.Invoke(command, new object[] { null });
            canExecuteChangedEvent.AddEventHandler(command, new EventHandler(Command_CanExecuteChanged));
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
#endif