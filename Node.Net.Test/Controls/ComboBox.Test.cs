using NUnit.Framework;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Media.Media3D;

namespace Node.Net.Controls
{
    [TestFixture]
    class ComboBoxTest
    {
        [Test, Explicit, Apartment(ApartmentState.STA)]
        public void ComboBox_Usage()
        {
            var cameras = new Dictionary<string, PerspectiveCamera>
            {
                { "Default", new PerspectiveCamera() },
                { "Up", new PerspectiveCamera {LookDirection = new Vector3D(0,0,1) } }
            };
            new Window
            {
                Title = "ComboBox Usage",
                Content = new AutoSizeGrid { Element = new ComboBox { DataContext = cameras } },
                WindowState = WindowState.Maximized
            }.ShowDialog();
        }
    }
}
