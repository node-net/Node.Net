namespace Node.Net.View
{
    [NUnit.Framework.TestFixture]
    class DynamicView_Test
    {
        [NUnit.Framework.TestCase,NUnit.Framework.RequiresSTA,NUnit.Framework.Explicit]
        public void DynamicView_Usage()
        {
            DynamicView dynamicView = new DynamicView(new Properties());
            dynamicView.DataContext = new Widget();
            Node.Net.View.Window.ShowDialog(dynamicView, "DynamicView_Usage");
        }

        class Foo
        {
            private string name = "";
            public string Name
            {
                get { return name; }
                set { name = value; }
            }
        }
        class Bar : Foo
        {
            private string state = "CO";
            public string State
            {
                get { return state;}
                set { state = value; }
            }
        }
        [NUnit.Framework.TestCase,NUnit.Framework.RequiresSTA]
        public void DynamicView_ByType_Usage()
        {
            DynamicView dynamicView = new DynamicView(new Properties());
            dynamicView.Elements.Add("ListView", new ListView());
            dynamicView.Elements.Add("TextView", new TextView());
            dynamicView.TypeNames.Add(typeof(Foo), "ListView");
            dynamicView.TypeNames.Add(typeof(Bar), "TextView");
            dynamicView.DataContext = new Widget();
            NUnit.Framework.Assert.AreSame(dynamicView.Elements["Default"], dynamicView.Content);
            dynamicView.DataContext = new Foo();
            NUnit.Framework.Assert.AreSame(dynamicView.Elements["ListView"], dynamicView.Content);
            dynamicView.DataContext = new Bar();
            NUnit.Framework.Assert.AreSame(dynamicView.Elements["TextView"], dynamicView.Content);
            //System.Windows.Window window = new System.Windows.Window();
            //window.Content = dynamicView;
            //window.Title = "DynamicView_Usage";
            //window.ShowDialog();
        }
    }
}
