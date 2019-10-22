using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Node.Net.Factory.Test.Factories.TypeFactories
{
    [TestFixture]
    class ColorFactoryTest
    {
        [Test]
        public void ColorFactory_Usage()
        {
            var factory = new Node.Net.Factory.Factories.TypeFactories.ColorFactory();
            Assert.AreEqual(Colors.Black, factory.Create("?"));
        }
    }
}
