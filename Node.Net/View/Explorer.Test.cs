using NUnit.Framework;

namespace Node.Net.View
{
    [TestFixture,Category("Node.Net.View.Explorer")]
    class Explorer_Test
    {
        [TestCase,NUnit.Framework.Apartment(System.Threading.ApartmentState.STA),NUnit.Framework.Explicit]
        public void Explorer_Properties_Horizontal()
        {
            System.Collections.Generic.Dictionary<string, object> doc =
                new System.Collections.Generic.Dictionary<string, object>();
            doc.Add("Name", "testDoc");
            System.Collections.Generic.Dictionary<string, object> child =
                new System.Collections.Generic.Dictionary<string, object>();
            child.Add("Name", "childA");
            doc.Add("childA", child);
            Node.Net.Json.Hash hash = new Node.Net.Json.Hash();
            for (int i = 0; i < 500; i++)
            {
                hash[i.ToString()] = i;
            }
            doc.Add("childB", hash);

            System.Windows.FrameworkElement[] elements
                = { new Controls.TreeView(), new Controls.PropertyControl() };
            System.Windows.Window window = new System.Windows.Window()
            {
                Content = new Explorer(null,new Controls.PropertyControl(), System.Windows.Controls.Orientation.Horizontal),
                Title = "Explorer_Properties",
                DataContext = new System.Collections.Generic.KeyValuePair<string, object>("doc", doc),
                WindowState=System.Windows.WindowState.Maximized
            };
            window.ShowDialog();
        }

        [TestCase,NUnit.Framework.Apartment(System.Threading.ApartmentState.STA), NUnit.Framework.Explicit]
        public void Explorer_DynamicViewSelector()
        {
            DynamicViewSelector dynamicViewSelector = new DynamicViewSelector(new Controls.PropertyControl());
            dynamicViewSelector.DynamicView.Elements.Add("ListView", new ListView());
            dynamicViewSelector.DynamicView.Elements.Add("TreeView", new Controls.TreeView());
            dynamicViewSelector.DynamicView.Elements.Add("TextView", new TextView());
            //dynamicViewSelector.DataContext = new Widget();
            System.Windows.Window window = new System.Windows.Window()
            {
                Content = new Explorer(null, dynamicViewSelector),
                Title = "Explorer_DynamicViewSelector",
                DataContext = new Widget()
            };
            window.ShowDialog();
        }
    }
}
