using NUnit.Framework;

namespace Node.Net.View
{
    [TestFixture,Category("Node.Net.View.TabControl")]
    class TabControl_Test
    {
        [TestCase, Apartment(System.Threading.ApartmentState.STA), Explicit]
        public void TabControl_Usage()
        {
            var model = new Node.Net.Deprecated.Collections.Hash();
            model["Name"] = nameof(model);
            model["Integer"] = 3;

            var widget = new Node.Net.Deprecated.Collections.Hash();
            widget["Name"] = nameof(widget);
            model[nameof(Widget)] = widget;

            var controls = new Node.Net.Deprecated.Collections.Hash();
            controls[nameof(Explorer)] = new Explorer { DataContext = model };
            controls[nameof(Properties)] = new Deprecated.Controls.PropertyControl { DataContext = model };
            controls[nameof(JsonView)] = new Deprecated.Controls.ReadOnlyTextBox { DataContext = model };

            var tabControl = new TabControl
            {
                TabStripPlacement = System.Windows.Controls.Dock.Bottom,
                DataContext = controls
            };
            Node.Net.View.Window.ShowDialog(tabControl, "TabControl.Test");
        }
    }
}
