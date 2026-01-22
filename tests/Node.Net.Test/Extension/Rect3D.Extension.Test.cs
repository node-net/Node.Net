using System.Threading.Tasks;
using Node.Net; // Extension methods are in Node.Net namespace

namespace Node.Net.Test.Extension
{
    internal class Rect3DExtensionTest
    {
        [Test]
        public async Task Usage()
        {
            Rect3D rect = new Rect3D();
            rect.GetCenter();
            rect.Scale(1.1);
            await Task.CompletedTask;
        }
    }
}