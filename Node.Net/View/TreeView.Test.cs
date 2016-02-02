
using NUnit.Framework;
namespace Node.Net.View
{
    [TestFixture,NUnit.Framework.Category("TreeView")]
    class TreeView_Test
    {
        [TestCase, Apartment(System.Threading.ApartmentState.STA), NUnit.Framework.Explicit]
        public void TreeView_Usage()
        {
            System.Collections.Generic.Dictionary<string, object> doc =
                new System.Collections.Generic.Dictionary<string, object>();
            doc.Add("Name", "testDoc");
            System.Collections.Generic.Dictionary<string, object> child =
                new System.Collections.Generic.Dictionary<string, object>();
            child.Add("Name", "childA");
            doc.Add("childA", child);

            System.Collections.Generic.KeyValuePair<string, object> kvp
                = new System.Collections.Generic.KeyValuePair<string, object>("doc", doc);

            Controls.TreeView treeView = new Controls.TreeView(kvp);
            System.Windows.Window window = new System.Windows.Window() { Content = treeView, Title = "SDIApplication" };
            window.ShowDialog();
        }

        public class XmlTreeViewItem : Controls.TreeViewItem
        {
            public XmlTreeViewItem() : base(null) { }
            public XmlTreeViewItem(object value) : base(value) { }
            public XmlTreeViewItem(object model, int childDepth) : base(model,childDepth)
            {
            }
            /*
            protected override object GetHeader()
            {
                object context = Node.Net.View.KeyValuePair.GetValue(DataContext);
                System.Xml.XmlNode xmlNode = context as System.Xml.XmlNode;
                System.Xml.XmlDocument xmlDocument = context as System.Xml.XmlDocument;
                if (!object.ReferenceEquals(null, xmlNode) && 
                    object.ReferenceEquals(null,xmlDocument))
                {
                    return xmlNode.Name;
                }
                return base.GetHeader();
            }*/
        }

        [TestCase, NUnit.Framework.Apartment(System.Threading.ApartmentState.STA), NUnit.Framework.Explicit]
        public void TreeView_Usage_Xml()
        {
            System.Xml.XmlDocument xdoc = new System.Xml.XmlDocument();
            System.Xml.XmlElement elementA = xdoc.CreateElement("A");
            xdoc.AppendChild(elementA);
            //xdoc.DocumentElement = elementA;
            //xdoc.DocumentElement.AppendChild(elementA);
            elementA.AppendChild(xdoc.CreateElement("B"));

            System.Collections.Generic.KeyValuePair<string, object> kvp
                = new System.Collections.Generic.KeyValuePair<string, object>("doc", xdoc);

            Controls.TreeView treeView = new Controls.TreeView(kvp);
            treeView.TreeViewItemType = typeof(XmlTreeViewItem);
            System.Windows.Window window = new System.Windows.Window() { Content = treeView, Title = "SDIApplication" };
            window.ShowDialog();
        }

        [TestCase, NUnit.Framework.Apartment(System.Threading.ApartmentState.STA), NUnit.Framework.Explicit]
        public void TreeView_Usage_MultipleRootElements()
        {
            System.Collections.Generic.Dictionary<string, string> settings
                = new System.Collections.Generic.Dictionary<string, string>();
            settings.Add("Database", "MyData.sql");

            System.Collections.Generic.Dictionary<string, object> doc =
                new System.Collections.Generic.Dictionary<string, object>();
            doc.Add("Name", "testDoc");
            System.Collections.Generic.Dictionary<string, object> child =
                new System.Collections.Generic.Dictionary<string, object>();
            child.Add("Name", "childA");
            doc.Add("childA", child);

            System.Collections.Generic.KeyValuePair<string, object> kvp1
                = new System.Collections.Generic.KeyValuePair<string, object>("settings", settings);
            System.Collections.Generic.KeyValuePair<string, object> kvp2
                = new System.Collections.Generic.KeyValuePair<string, object>("doc", doc);
            object[] items = { kvp1, kvp2 };

            Controls.TreeView treeView = new Controls.TreeView(items);
            System.Windows.Window window = new System.Windows.Window() { Content = treeView, Title = "SDIApplication" };
            window.ShowDialog();
        }
    }
}
