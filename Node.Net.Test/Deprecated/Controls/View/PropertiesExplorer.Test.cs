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
            var doc =
                new System.Collections.Generic.Dictionary<string, object>();
            doc.Add("Name", "testDoc");
            var child =
                new System.Collections.Generic.Dictionary<string, object>();
            child.Add("Name", "childA");
            doc.Add("childA", child);
            var hash = new Node.Net.Deprecated.Collections.Hash();
            for (int i = 0; i < 500; i++)
            {
                hash[i.ToString()] = i;
            }
            doc.Add("childB", hash);

            System.Windows.FrameworkElement[] elements
                = { new Deprecated.Controls.TreeView(), new Deprecated.Controls.PropertyControl() };
            var window = new System.Windows.Window
            {
                Content = new PropertiesExplorer(null, new Deprecated.Controls.PropertyControl(), System.Windows.Controls.Orientation.Horizontal),
                Title = "Explorer_Properties",
                DataContext = new System.Collections.Generic.KeyValuePair<string, object>(nameof(doc), doc)
            };
            window.ShowDialog();
        }

        [TestCase, NUnit.Framework.Apartment(System.Threading.ApartmentState.STA), NUnit.Framework.Explicit]
        public void PropertiesExplorer_Properties_Vertical()
        {
            var doc =
                new System.Collections.Generic.Dictionary<string, object>();
            doc.Add("Name", "testDoc");
            var child =
                new System.Collections.Generic.Dictionary<string, object>();
            child.Add("Name", "childA");
            doc.Add("childA", child);
            var hash = new Node.Net.Deprecated.Collections.Hash();
            for (int i = 0; i < 500; i++)
            {
                hash[i.ToString()] = i;
            }
            doc.Add("childB", hash);

            System.Windows.FrameworkElement[] elements
                = { new Deprecated.Controls.TreeView(), new Deprecated.Controls.PropertyControl() };
            var window = new System.Windows.Window
            {
                Content = new PropertiesExplorer(null, new Deprecated.Controls.PropertyControl(), System.Windows.Controls.Orientation.Vertical),
                Title = "Explorer_Properties",
                DataContext = new System.Collections.Generic.KeyValuePair<string, object>(nameof(doc), doc)
            };
            window.ShowDialog();
        }


        [TestCase,NUnit.Framework.Apartment(System.Threading.ApartmentState.STA), NUnit.Framework.Explicit]
        public void Explorer_DynamicViewSelector()
        {
            var dynamicViewSelector = new DynamicViewSelector(new Deprecated.Controls.PropertyControl());
            dynamicViewSelector.DynamicView.Elements.Add(nameof(ListView), new ListView());
            dynamicViewSelector.DynamicView.Elements.Add(nameof(TreeView), new Deprecated.Controls.TreeView());
            dynamicViewSelector.DynamicView.Elements.Add(nameof(TextView), new TextView());
            //dynamicViewSelector.DataContext = new Widget();
            var window = new System.Windows.Window
            {
                Content = new Explorer(null, dynamicViewSelector),
                Title = nameof(Explorer_DynamicViewSelector),
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
            var hash = new Deprecated.Collections.Hash(GetStream("PropertiesExplorer.Test.ExampleA.json"));
            using (var propertyControl = new Deprecated.Controls.PropertyControl())
            {
                System.Windows.FrameworkElement[] elements
                    = { new Deprecated.Controls.TreeView(), propertyControl };
                var window = new System.Windows.Window
                {
                    Content = new PropertiesExplorer(null, new Deprecated.Controls.PropertyControl(), System.Windows.Controls.Orientation.Vertical),
                    Title = "Explorer_Properties",
                    DataContext = new System.Collections.Generic.KeyValuePair<string, object>("ExampleA", hash)
                };
                window.ShowDialog();
            }

        }

    }
}
