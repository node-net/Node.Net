using NUnit.Framework;
namespace Node.Net.View
{
    [TestFixture,NUnit.Framework.Category(nameof(SDIMainControl))]
    class SDIMainControl_Test
    {
        [TestCase, NUnit.Framework.Apartment(System.Threading.ApartmentState.STA), NUnit.Framework.Explicit]
        public void SDIMainControl_Notepad_Usage()
        {
            var sdiMainControl
                = new SDIMainControl(
                    typeof(NotePadDocument),
                    new TextView()) { ApplicationName = "SDINotepad" };
            sdiMainControl.ShowDialog();
        }

        [TestCase, NUnit.Framework.Apartment(System.Threading.ApartmentState.STA), NUnit.Framework.Explicit]
        public void SDMainControl_Properties_Update_Usage()
        {
            var sdiMainControl
                = new SDIMainControl(
                    typeof(Node.Net.Deprecated.Collections.Document),
                    new Explorer(new Deprecated.Controls.TreeView(), new Deprecated.Controls.PropertyControl())) { ApplicationName = "SDIProperties" };
            sdiMainControl.ShowDialog();
        }

        [TestCase, NUnit.Framework.Apartment(System.Threading.ApartmentState.STA), NUnit.Framework.Explicit]
        public void SDIMainControl_Usage_2()
        {
            var settings
       = new System.Collections.Generic.Dictionary<string, string>();
            settings.Add("Database", "MyData.sql");

            var hash = new Node.Net.Deprecated.Collections.Hash();
            for (int i = 0; i < 500; i++)
            {
                hash[i.ToString()] = i;
            }

            object[] items = {
                new System.Collections.Generic.KeyValuePair<string,object>("Settings",settings),
                new System.Collections.Generic.KeyValuePair<string,object>("LargeHash",hash),
                new System.Collections.Generic.KeyValuePair<string,object>("Untitled",null)};//new NotePadDocument()) };

            var dynamicView = new DynamicView(new Deprecated.Controls.PropertyControl());
            dynamicView.Elements.Add(nameof(TextView), new TextView());
            dynamicView.TypeNames.Add(typeof(NotePadDocument), nameof(TextView));
            var sdiMainControl
                = new SDIMainControl(
                    typeof(NotePadDocument),
                    new Explorer(new Deprecated.Controls.TreeView(), dynamicView))
                {
                    ApplicationName = "SDINotepad",
                    DataContext = items
                };
            sdiMainControl.ShowDialog();
        }

        public class CustomTreeViewItem : Deprecated.Controls.TreeViewItem
        {
            public CustomTreeViewItem() :base(null){ }
            public CustomTreeViewItem(object value) : base(value) { }
            protected override object GetHeader() => "<Custom> " + base.GetHeader();
        }
        [TestCase, NUnit.Framework.Apartment(System.Threading.ApartmentState.STA), NUnit.Framework.Explicit]
        public void SDIMainControl_Usage_CustomTreeViewItem()
        {
            var settings
       = new System.Collections.Generic.Dictionary<string, string>();
            settings.Add("Database", "MyData.sql");

            var hash = new Node.Net.Deprecated.Collections.Hash();
            for (int i = 0; i < 500; i++)
            {
                hash[i.ToString()] = i;
            }

            object[] items = {
                new System.Collections.Generic.KeyValuePair<string,object>("Settings",settings),
                new System.Collections.Generic.KeyValuePair<string,object>("LargeHash",hash),
                new System.Collections.Generic.KeyValuePair<string,object>("Untitled",null)};//new NotePadDocument()) };

            var dynamicView = new DynamicView(new Deprecated.Controls.PropertyControl());
            dynamicView.Elements.Add(nameof(TextView), new TextView());
            dynamicView.TypeNames.Add(typeof(NotePadDocument), nameof(TextView));
            var sdiMainControl
                = new SDIMainControl(
                    typeof(NotePadDocument),
                    new Explorer(new Deprecated.Controls.TreeView(typeof(CustomTreeViewItem)), dynamicView))
                {
                    ApplicationName = "SDINotepad",
                    DataContext = items
                };
            sdiMainControl.ShowDialog();
        }
    }
}