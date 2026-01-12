using NUnit.Framework;

namespace Node.Net.Test
{
    [TestFixture]
    internal class LogTest
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