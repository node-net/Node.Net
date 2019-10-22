using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace Node.Net.Factories.Test
{
    [TestFixture]
    class ExtensionsTest
    {
        [TestCase("Mesh.Square.xaml")]
        public void Clone(string name)
        {
            var value = XamlReader.Load(GlobalFixture.GetStream(name));
            Assert.NotNull(value);
            if(typeof(System.Windows.Freezable).IsAssignableFrom(value.GetType()))
            {
                var clone = (value as System.Windows.Freezable).Clone();
                Assert.NotNull(clone);
                Assert.AreNotSame(value, clone);
            }
        }
    }
}
