using System.Threading.Tasks;
using Node.Net;

namespace Node.Net.Test
{
    internal class LogTest
    {
        [Test]
        public async Task Usage()
        {
            Log.Info("info");
            Log.Debug("debug");
            Log.Error("error");
            Log.Fatal("fatal");
            Log.Warn("warn");
            await Task.CompletedTask;
        }
    }
}