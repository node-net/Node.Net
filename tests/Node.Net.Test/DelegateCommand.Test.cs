#if IS_WINDOWS
using System;
using System.Threading.Tasks;
using Node.Net;

namespace Node.Net.Test
{
    internal class DelegateCommandTest
    {
        [Test]
        public async Task Usage()
        {
            // Use reflection to access conditionally compiled type
            var assembly = typeof(Factory).Assembly;
            var delegateCommandType = assembly.GetType("Node.Net.DelegateCommand");
            if (delegateCommandType == null)
            {
                // TUnit doesn't have Assert.Pass - just return early
                return;
            }
            
            var command = System.Activator.CreateInstance(delegateCommandType, new Action<object>(TestCommand));
            var executeMethod = delegateCommandType.GetMethod("Execute");
            var canExecuteMethod = delegateCommandType.GetMethod("CanExecute");
            var canExecuteChangedEvent = delegateCommandType.GetEvent("CanExecuteChanged");
            
            executeMethod.Invoke(command, new object[] { null });
            await Assert.That((bool)canExecuteMethod.Invoke(command, new object[] { null })).IsTrue();

            command = System.Activator.CreateInstance(delegateCommandType, new Action<object>(TestCommand), new Func<object, bool>(TestCanExecute));
            await Assert.That((bool)canExecuteMethod.Invoke(command, new object[] { null })).IsTrue();
            executeMethod.Invoke(command, new object[] { null });

            command = System.Activator.CreateInstance(delegateCommandType, new object[] { null });
            executeMethod.Invoke(command, new object[] { null });
            canExecuteChangedEvent.AddEventHandler(command, new EventHandler(Command_CanExecuteChanged));
            await Task.CompletedTask;
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