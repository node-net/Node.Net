using NUnit.Framework;
namespace Node.Net.View
{
    [TestFixture]
    class DynamicView_Test
    {
        [TestCase,NUnit.Framework.Apartment(System.Threading.ApartmentState.STA),NUnit.Framework.Explicit]
        public void DynamicView_Usage()
        {
            var dynamicView = new DynamicView(new Deprecated.Controls.PropertyControl())
            {
                DataContext = new Widget()
            };
            Node.Net.View.Window.ShowDialog(dynamicView, nameof(DynamicView_Usage));
        }

        class Foo
        {
            public string Name { get; set; } = "";
        }
        class Bar : Foo
        {
            public string State { get; set; } = "CO";
        }
        [TestCase,NUnit.Framework.Apartment(System.Threading.ApartmentState.STA)]
        public void DynamicView_ByType_Usage()
        {
            var dynamicView = new DynamicView(new Deprecated.Controls.PropertyControl());
            dynamicView.Elements.Add(nameof(ListView), new ListView());
            dynamicView.Elements.Add(nameof(TextView), new TextView());
            dynamicView.TypeNames.Add(typeof(Foo), nameof(ListView));
            dynamicView.TypeNames.Add(typeof(Bar), nameof(TextView));
            dynamicView.DataContext = new Widget();
            Assert.AreSame(dynamicView.Elements["Default"], dynamicView.Content);
            dynamicView.DataContext = new Foo();
            Assert.AreSame(dynamicView.Elements[nameof(ListView)], dynamicView.Content);
            dynamicView.DataContext = new Bar();
            Assert.AreSame(dynamicView.Elements[nameof(TextView)], dynamicView.Content);
            //System.Windows.Window window = new System.Windows.Window();
            //window.Content = dynamicView;
            //window.Title = "DynamicView_Usage";
            //window.ShowDialog();
        }
    }
}
