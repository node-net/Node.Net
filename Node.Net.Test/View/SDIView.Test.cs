using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using NUnit.Framework;

namespace Node.Net.View
{
	[TestFixture]
	public class SDIViewTest
	{
		//[Test,Explicit,Apartment(ApartmentState.STA)]
		public void ShowDialog()
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
				WindowState=WindowState.Maximized,
				Content = new SDIView(),
				DataContext = vm
			}.ShowDialog();
		}
	}
}
