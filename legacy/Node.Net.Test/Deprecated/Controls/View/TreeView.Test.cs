
using NUnit.Framework;
namespace Node.Net.View
{
    [TestFixture,NUnit.Framework.Category(nameof(TreeView))]
    class TreeView_Test
    {
        [TestCase, Apartment(System.Threading.ApartmentState.STA), NUnit.Framework.Explicit]
        public void TreeView_Usage()
        {
            var doc =
                new System.Collections.Generic.Dictionary<string, object>();
            doc.Add("Name", "testDoc");
            var child =
                new System.Collections.Generic.Dictionary<string, object>();
            child.Add("Name", "childA");
            doc.Add("childA", child);

            var kvp
                = new System.Collections.Generic.KeyValuePair<string, object>(nameof(doc), doc);

            var treeView = new Deprecated.Controls.TreeView(kvp);
            var window = new System.Windows.Window { Content = treeView, Title = nameof(SDIApplication) };
            window.ShowDialog();
        }

        public class XmlTreeViewItem : Deprecated.Controls.TreeViewItem
        {
            public XmlTreeViewItem() : base(null) { }
            public XmlTreeViewItem(object value) : base(value) { }
            public XmlTreeViewItem(object model, int childDepth) : base(model,childDepth)
            {
            }
        }

        [TestCase, NUnit.Framework.Apartment(System.Threading.ApartmentState.STA), NUnit.Framework.Explicit]
        public void TreeView_Usage_Xml()
        {
            var xdoc = new System.Xml.XmlDocument();
            var elementA = xdoc.CreateElement("A");
            xdoc.AppendChild(elementA);
            //xdoc.DocumentElement = elementA;
            //xdoc.DocumentElement.AppendChild(elementA);
            elementA.AppendChild(xdoc.CreateElement("B"));

            var kvp
                = new System.Collections.Generic.KeyValuePair<string, object>("doc", xdoc);

            var treeView = new Deprecated.Controls.TreeView(kvp)
            {
                TreeViewItemType = typeof(XmlTreeViewItem)
            };
            var window = new System.Windows.Window { Content = treeView, Title = nameof(SDIApplication) };
            window.ShowDialog();
        }

        [TestCase, NUnit.Framework.Apartment(System.Threading.ApartmentState.STA), NUnit.Framework.Explicit]
        public void TreeView_Usage_MultipleRootElements()
        {
            var settings
                = new System.Collections.Generic.Dictionary<string, string>();
            settings.Add("Database", "MyData.sql");

            var doc =
                new System.Collections.Generic.Dictionary<string, object>();
            doc.Add("Name", "testDoc");
            var child =
                new System.Collections.Generic.Dictionary<string, object>();
            child.Add("Name", "childA");
            doc.Add("childA", child);

            var kvp1
                = new System.Collections.Generic.KeyValuePair<string, object>(nameof(settings), settings);
            var kvp2
                = new System.Collections.Generic.KeyValuePair<string, object>(nameof(doc), doc);
            object[] items = { kvp1, kvp2 };

            var treeView = new Deprecated.Controls.TreeView(items);
            var window = new System.Windows.Window { Content = treeView, Title = nameof(SDIApplication) };
            window.ShowDialog();
        }
    }
}
