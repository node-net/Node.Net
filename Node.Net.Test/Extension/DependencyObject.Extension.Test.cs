using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
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

            var border = Factory.Default.Create<Border>("Border.Button.xaml");
            Assert.NotNull(border, nameof(border));

            buttons = border.Collect<Button>();
            Assert.AreEqual(1, buttons.Count, "border Button count");

            //var frame = Factory.Default.Create<Frame>("Frame.Button.xaml");
            //Assert.NotNull(frame, nameof(frame));

            /*
            var frameChildren = LogicalTreeHelper.GetChildren(frame);
            var children = new List<object>();
            foreach(var c in frameChildren)
            {
                children.Add(c);
            }
            Assert.AreEqual(1, children.Count,"frame logical children");
            */

            //buttons = frame.Collect<Button>();
            //Assert.AreEqual(1, buttons.Count,"frame Button Count");

            grid = Factory.Default.Create<Grid>("Grid.Deep.Buttons.xaml");
            Assert.NotNull(grid, nameof(grid));

            buttons = grid.Collect<Button>();
            Assert.AreEqual(6, buttons.Count);
        }
    }
}
