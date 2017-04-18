using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Controls;

namespace Node.Net
{
    [TestFixture]
    class DependencyObjectExtensionTest
    {
        [Test,Apartment(ApartmentState.STA)]
        public void Collect()
        {
            var grid = Factory.Default.Create<Grid>("Grid.Buttons.xaml");
            Assert.NotNull(grid, nameof(grid));

            var buttons = grid.Collect<Button>();
            Assert.AreEqual(2, buttons.Count);

            grid = Factory.Default.Create<Grid>("Grid.Deep.Buttons.xaml");
            Assert.NotNull(grid, nameof(grid));

            buttons = grid.Collect<Button>();
            Assert.AreEqual(4, buttons.Count);
        }
    }
}
