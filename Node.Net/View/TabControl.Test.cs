using NUnit.Framework;

namespace Node.Net.View
{
    [TestFixture,Category("Node.Net.View.TabControl")]
    class TabControl_Test
    {
        [TestCase, Apartment(System.Threading.ApartmentState.STA), Explicit]
        public void TabControl_Usage()
        {
            Node.Net.Json.Hash model = new Node.Net.Json.Hash();
            model["Name"] = "model";
            model["Integer"] = 3;

            Node.Net.Json.Hash widget = new Node.Net.Json.Hash();
            widget["Name"] = "widget";
            model["Widget"] = widget;

            Node.Net.Json.Hash controls = new Node.Net.Json.Hash();
            controls["Explorer"] = new Explorer() { DataContext = model };
            controls["Properties"] = new Properties() { DataContext = model };
            controls["JsonView"] = new Controls.ReadOnlyTextBox() { DataContext = model };

            TabControl tabControl = new TabControl() { TabStripPlacement=System.Windows.Controls.Dock.Bottom};
            tabControl.DataContext = controls;
            Node.Net.View.Window.ShowDialog(tabControl, "TabControl.Test");
        }
    }
}
