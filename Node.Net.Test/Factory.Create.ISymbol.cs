using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using NUnit.Framework;
using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace Node.Net.Tests
{
    [TestFixture]
    public class FactoryCreateISymbolTest
    {
        private Factory factory;
        [SetUp]
        public void SetUp()
        {
            factory = new Factory();
            factory.ManifestResourceAssemblies.Add(typeof(FactoryCreateISymbolTest).Assembly);
        }
        [Test,Apartment(ApartmentState.STA)]
        [TestCase("Scene.500.json")]
        public void CreateFromManifestResourceStream(string name)
        {
            var data = factory.Create<IDictionary>(name);
            Assert.NotNull(data, nameof(data));

            var symbol = factory.Create<ISymbol>(data);
            Assert.NotNull(symbol, nameof(symbol));
        }
        [Test, Apartment(ApartmentState.STA),Explicit]
        [TestCase("Scene.500.json")]
        [TestCase("Foo.0.json")]
        public void Create_ShowDialog(string name)
        {
            var data = factory.Create<IDictionary>(name);
            Assert.NotNull(data, nameof(data));

            var symbol = factory.Create<ISymbol>(data);
            Assert.NotNull(symbol, nameof(symbol));

            var window = new Window
            {
                Title = $"{name} ISymbol",
                WindowState = WindowState.Maximized,
                Content = new Button { Content = symbol as UIElement }
            };
            window.ShowDialog();
        }
    }
}
