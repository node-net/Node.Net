using NUnit.Framework;
namespace Node.Net.View
{
    [TestFixture,NUnit.Framework.Category("SDIMainControl")]
    class SDIMainControl_Test
    {
        [TestCase, NUnit.Framework.Apartment(System.Threading.ApartmentState.STA), NUnit.Framework.Explicit]
        public void SDIMainControl_Notepad_Usage()
        {
            SDIMainControl sdiMainControl
                = new SDIMainControl(
                    typeof(NotePadDocument),
                    new TextView()) { ApplicationName = "SDINotepad" };
            sdiMainControl.ShowDialog();
        }

        [TestCase, NUnit.Framework.Apartment(System.Threading.ApartmentState.STA), NUnit.Framework.Explicit]
        public void SDMainControl_Properties_Update_Usage()
        {
            SDIMainControl sdiMainControl
                = new SDIMainControl(
                    typeof(Node.Net.Json.Document),
                    new Explorer(new TreeView(), new Properties())) { ApplicationName = "SDIProperties" };
            sdiMainControl.ShowDialog();
        }

        [TestCase, NUnit.Framework.Apartment(System.Threading.ApartmentState.STA), NUnit.Framework.Explicit]
        public void SDIMainControl_Usage_2()
        {
            System.Collections.Generic.Dictionary<string, string> settings
                = new System.Collections.Generic.Dictionary<string, string>();
            settings.Add("Database", "MyData.sql");

            Node.Net.Json.Hash hash = new Node.Net.Json.Hash();
            for (int i = 0; i < 500; i++)
            {
                hash[i.ToString()] = i;
            }

            object[] items = { 
                new System.Collections.Generic.KeyValuePair<string,object>("Settings",settings), 
                new System.Collections.Generic.KeyValuePair<string,object>("LargeHash",hash),
                new System.Collections.Generic.KeyValuePair<string,object>("Untitled",null)};//new NotePadDocument()) };

            DynamicView dynamicView = new DynamicView(new Properties());
            dynamicView.Elements.Add("TextView", new TextView());
            dynamicView.TypeNames.Add(typeof(NotePadDocument), "TextView");
            SDIMainControl sdiMainControl
                = new SDIMainControl(
                    typeof(NotePadDocument),
                    new Explorer(new TreeView(), dynamicView)) { ApplicationName = "SDINotepad" };
            sdiMainControl.DataContext = items;
            sdiMainControl.ShowDialog();
            //sdiMainForm.ShowDialog();
        }

        public class CustomTreeViewItem : TreeViewItem
        {
            public CustomTreeViewItem() :base(null){ }
            public CustomTreeViewItem(object value) : base(value) { }
            protected override object GetHeader() => "<Custom> " + base.GetHeader();
        }
        [TestCase, NUnit.Framework.Apartment(System.Threading.ApartmentState.STA), NUnit.Framework.Explicit]
        public void SDIMainControl_Usage_CustomTreeViewItem()
        {
            System.Collections.Generic.Dictionary<string, string> settings
                = new System.Collections.Generic.Dictionary<string, string>();
            settings.Add("Database", "MyData.sql");

            Node.Net.Json.Hash hash = new Node.Net.Json.Hash();
            for (int i = 0; i < 500; i++)
            {
                hash[i.ToString()] = i;
            }

            object[] items = { 
                new System.Collections.Generic.KeyValuePair<string,object>("Settings",settings), 
                new System.Collections.Generic.KeyValuePair<string,object>("LargeHash",hash),
                new System.Collections.Generic.KeyValuePair<string,object>("Untitled",null)};//new NotePadDocument()) };

            DynamicView dynamicView = new DynamicView(new Properties());
            dynamicView.Elements.Add("TextView", new TextView());
            dynamicView.TypeNames.Add(typeof(NotePadDocument), "TextView");
            SDIMainControl sdiMainControl
                = new SDIMainControl(
                    typeof(NotePadDocument),
                    new Explorer(new TreeView(typeof(CustomTreeViewItem)), dynamicView)) { ApplicationName = "SDINotepad" };
            sdiMainControl.DataContext = items;
            sdiMainControl.ShowDialog();
        }
    }
}