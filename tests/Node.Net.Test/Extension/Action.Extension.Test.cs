using System;
using System.Threading.Tasks;
using Node.Net; // Extension methods are in Node.Net namespace

namespace Node.Net.Test.Extension
{
    internal static class ActionExtensionTest
    {
        [Test]
        public static async Task Invoke()
        {
            Action<int> a1 = Invoke1;
            a1.Invoke(new object[] { 5 });

            Action<int, int> a2 = Invoke2;
            a2.Invoke(new object[] { 5, 6 });
            await Task.CompletedTask;
        }

        private static void Invoke1(int a) { }
        private static void Invoke2(int a, int b) { }
    }
}
