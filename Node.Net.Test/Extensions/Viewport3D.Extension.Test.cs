using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Media.Media3D;

namespace Node.Net.Extensions
{
    [TestFixture]
    class Viewport3DExtensionTest
    {
        [Test, Explicit, Apartment(ApartmentState.STA)]
        public void Viewport3D_Usage()
        {
            new Window
            {
                Title = "Viewport3D_Usage",
                Content = new Test.Viewport3DTester(),
                WindowState = WindowState.Maximized
            }.ShowDialog();
        }
    }
}
