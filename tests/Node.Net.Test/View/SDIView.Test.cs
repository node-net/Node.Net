#if IS_WINDOWS
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

namespace Node.Net.View
{
	public class SDIViewTest
	{
		[Test]
		public async Task ShowDialog()
		{
			var filename = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Node.Net.View.SDIViewTest.json";
			var vm = new SDIViewVM
			{
				FileName = filename,
				Views = new Collections.Items<FrameworkElement>(
					new List<FrameworkElement>
					{
						new View.JsonView()
					})
			};
			vm.Views.SelectedItem = vm.Views[0];
			new Window
			{
				Title = "SDIView Test",
				WindowState = WindowState.Maximized,
				Content = new SDIView(),
				DataContext = vm
			}.ShowDialog();
		}
	}
}
#endif
