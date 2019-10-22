using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Controls;

namespace Node.Net
{
    [TestFixture,Apartment(ApartmentState.STA)]
    class FactoryCreateTreeViewItemTest
    {
        [Test]
        public void Scene()
        {
            var factory = new Factory();
            factory.ManifestResourceAssemblies.Add(typeof(FactoryTest).Assembly);

            var scene = factory.Create<IDictionary>("Scene.json");
            Assert.NotNull(scene, nameof(scene));

            var tvi = factory.Create<TreeViewItem>(scene);
            Assert.NotNull(tvi, nameof(tvi));

            Assert.AreEqual(1, tvi.Items.Count);
        }
    }
}
