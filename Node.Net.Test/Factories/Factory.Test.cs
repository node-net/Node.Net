using NUnit.Framework;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Node.Net.Factories.Test
{
    [TestFixture]
    class FactoryTest
    {
        [Test]
        public void Factory_Usage()
        {
            var factory = new Factory();
            Assert.IsNull(factory.Create(typeof(object), null));
            Assert.IsNull(factory.Create(typeof(Color), "Blue"));

            factory.Add("Color", new ColorFactory());
            Assert.AreEqual(Colors.Blue, factory.Create(typeof(Color), "Blue"));

            factory.Add("Brush", new BrushFactory());
            var item = factory.Create(typeof(Brush), "Red");
            Assert.NotNull(item, nameof(item));
            Assert.True(typeof(Brush).IsAssignableFrom(item.GetType()), $"{item.GetType().FullName}");
            Assert.AreSame(Brushes.Red, factory.Create(typeof(Brush), "Red"));
            item = factory.Create(typeof(Brush), "255,0,0");
            var scb = item as SolidColorBrush;
            Assert.NotNull(scb, nameof(scb));

            factory.Add("Material", new MaterialFactory());
            Assert.NotNull(factory.Create(typeof(Material), "Blue"));
        }
        
        
    }
}
