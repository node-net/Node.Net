using NUnit.Framework;

namespace Node.Net.View
{
    [TestFixture]
    class SplitterGrid_Test
    {
        [TestCase,NUnit.Framework.Apartment(System.Threading.ApartmentState.STA), NUnit.Framework.Explicit]
        public void SplitterGrid_2_Vertical()
        {
            System.Windows.FrameworkElement[] elements
                = {new System.Windows.Controls.Label { Content = "pane 1" },
                   new System.Windows.Controls.Label { Content = "pane 2" }};

            var window = new System.Windows.Window
            {
                Content = new SplitterGrid(elements,System.Windows.Controls.Orientation.Vertical) ,
                Title = "View_ExplorerGrid_Usage" };
            window.ShowDialog();
        }
        [TestCase,NUnit.Framework.Apartment(System.Threading.ApartmentState.STA), NUnit.Framework.Explicit]
        public void SplitterGrid_3_Horizontal()
        {
            System.Windows.FrameworkElement[] elements
      = {new System.Windows.Controls.Label { Content = "pane 1" },
                   new System.Windows.Controls.Label { Content = "pane 2" },
                   new System.Windows.Controls.Label{Content="pane 3"}};

            var sgrid = new SplitterGrid(System.Windows.Controls.Orientation.Horizontal)
            {
                Elements = elements
            };
            var window = new System.Windows.Window
            {
                Content = sgrid,
                Title = "View_ExplorerGrid_Usage"
            };
            window.ShowDialog();
        }

        [TestCase,NUnit.Framework.Apartment(System.Threading.ApartmentState.STA), NUnit.Framework.Explicit]
        public void SplitterGrid_Nesting()
        {
            System.Windows.FrameworkElement[] velements
                = {new System.Windows.Controls.Label { Content = "pane A" },
                   new System.Windows.Controls.Label { Content = "pane B" }};

            System.Windows.FrameworkElement[] elements
               = { new SplitterGrid(velements,System.Windows.Controls.Orientation.Vertical),
                   new System.Windows.Controls.Label { Content = "pane 2" },
                   new System.Windows.Controls.Label{Content="pane 3"}};
            var window = new System.Windows.Window
            {
                Content = new SplitterGrid(elements, System.Windows.Controls.Orientation.Horizontal),
                Title = "View_ExplorerGrid_Usage"
            };
            window.ShowDialog();
        }

        [TestCase,NUnit.Framework.Apartment(System.Threading.ApartmentState.STA), NUnit.Framework.Explicit]
        public void SplitterGrid_TreeView_Properties()
        {
            var doc =
                new System.Collections.Generic.Dictionary<string, object>();
            doc.Add("Name", "testDoc");
            var child =
                new System.Collections.Generic.Dictionary<string, object>();
            child.Add("Name", "childA");
            doc.Add("childA", child);

            using (var propertyControl = new Deprecated.Controls.PropertyControl())
            {
                System.Windows.FrameworkElement[] elements
                    = { new Deprecated.Controls.TreeView(), propertyControl };
                var window = new System.Windows.Window
                {
                    Content = new SplitterGrid(elements, System.Windows.Controls.Orientation.Horizontal),
                    Title = "View_ExplorerGrid_Usage",
                    DataContext = new System.Collections.Generic.KeyValuePair<string, object>(nameof(doc), doc)
                };
                window.ShowDialog();
            }

        }
    }

}
