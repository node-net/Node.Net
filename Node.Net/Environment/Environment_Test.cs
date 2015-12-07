using NUnit.Framework;

namespace Node.Net.Environment
{
    [TestFixture, Category("Node.Net.Environment.Environment")]
    class Environment_Test
    {
        [TestCase]
        public void Environment_GetDevRoot()
        {
            Assert.True(System.IO.Directory.Exists(Environment.DevRoot));
        }
        [TestCase]
        public void Environment_GetWorkingDirectory()
        {
            Assert.True(System.IO.Directory.Exists(
                Node.Net.Environment.Environment.GetWorkingDirectory(typeof(Node.Net.Environment.Environment))));
            string dir = Environment.GetWorkingDirectory(typeof(Node.Net.Environment.Environment_Test));
            Assert.True(System.IO.Directory.Exists(
                Environment.GetWorkingDirectory(typeof(Node.Net.Environment.Environment_Test))));
        }
        
    }
}
