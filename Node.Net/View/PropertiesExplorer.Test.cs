using NUnit.Framework;
using System.IO;

namespace Node.Net.View
{
    [TestFixture,Category("Node.Net.View.PropertiesExplorer")]
    class PropertiesExplorer_Test
    {
        [TestCase,NUnit.Framework.Apartment(System.Threading.ApartmentState.STA), NUnit.Framework.Explicit]
        public void PropertiesExplorer_Properties_Horizontal()
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
                = { new TreeView(), new Properties() };
            System.Windows.Window window = new System.Windows.Window()
            {
                Content = new PropertiesExplorer(null, new Properties(), System.Windows.Controls.Orientation.Horizontal),
                Title = "Explorer_Properties",
                DataContext = new System.Collections.Generic.KeyValuePair<string, object>("doc", doc)
            };
            window.ShowDialog();
        }

        [TestCase, NUnit.Framework.Apartment(System.Threading.ApartmentState.STA), NUnit.Framework.Explicit]
        public void PropertiesExplorer_Properties_Vertical()
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
                = { new TreeView(), new Properties() };
            System.Windows.Window window = new System.Windows.Window()
            {
                Content = new PropertiesExplorer(null, new Properties(), System.Windows.Controls.Orientation.Vertical),
                Title = "Explorer_Properties",
                DataContext = new System.Collections.Generic.KeyValuePair<string, object>("doc", doc)
            };
            window.ShowDialog();
        }


        [TestCase,NUnit.Framework.Apartment(System.Threading.ApartmentState.STA), NUnit.Framework.Explicit]
        public void Explorer_DynamicViewSelector()
        {
            DynamicViewSelector dynamicViewSelector = new DynamicViewSelector(new Properties());
            dynamicViewSelector.DynamicView.Elements.Add("ListView", new ListView());
            dynamicViewSelector.DynamicView.Elements.Add("TreeView", new TreeView());
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

        private Stream GetStream(string name)
        {
            foreach(string resourceName in GetType().Assembly.GetManifestResourceNames())
            {
                if (resourceName.Contains(name)) return GetType().Assembly.GetManifestResourceStream(resourceName);
            }
            return null;
        }
        [TestCase, NUnit.Framework.Apartment(System.Threading.ApartmentState.STA), NUnit.Framework.Explicit]
        public void PropertiesExplorer_Json_Properties_Vertical()
        {
            Json.Hash hash = new Json.Hash(GetStream("PropertiesExplorer.Test.ExampleA.json"));
            System.Windows.FrameworkElement[] elements
                = { new TreeView(), new Properties() };
            System.Windows.Window window = new System.Windows.Window()
            {
                Content = new PropertiesExplorer(null, new Properties(), System.Windows.Controls.Orientation.Vertical),
                Title = "Explorer_Properties",
                DataContext = new System.Collections.Generic.KeyValuePair<string, object>("ExampleA", hash)
            };
            window.ShowDialog();
        }

    }
}
