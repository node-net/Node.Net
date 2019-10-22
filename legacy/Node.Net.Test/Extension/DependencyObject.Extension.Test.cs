using NUnit.Framework;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace Node.Net
{
	[TestFixture, Category(nameof(DependencyObject))]
	class DependencyObjectExtensionTest
	{
		[Test, Apartment(ApartmentState.STA)]
		public void Clone()
		{
			var grid = new Grid();
			Assert.NotNull(grid, nameof(grid));
			var clone = grid.Clone();
			Assert.NotNull(clone, nameof(clone));
		}
		[Test, Apartment(ApartmentState.STA)]
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

			grid = Factory.Default.Create<Grid>("Grid.Deep.Buttons.xaml");
			Assert.NotNull(grid, nameof(grid));

			buttons = grid.Collect<Button>();
			Assert.AreEqual(6, buttons.Count);
		}
	}
}
