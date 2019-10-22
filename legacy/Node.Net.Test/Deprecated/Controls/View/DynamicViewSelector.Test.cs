using NUnit.Framework;

namespace Node.Net.View
{
    [TestFixture]
    class DynamicViewSelector_Test
    {
        [TestCase,NUnit.Framework.Apartment(System.Threading.ApartmentState.STA),NUnit.Framework.Explicit]
        public void DynamicViewSelector_Usage()
        {
            var dynamicViewSelector = new DynamicViewSelector(new Deprecated.Controls.PropertyControl());
            dynamicViewSelector.DynamicView.Elements.Add(nameof(ListView), new ListView());
            dynamicViewSelector.DynamicView.Elements.Add(nameof(TreeView), new Deprecated.Controls.TreeView());
            dynamicViewSelector.DynamicView.Elements.Add(nameof(TextView), new TextView());
            dynamicViewSelector.DataContext = new Widget();
            var window = new System.Windows.Window
            {
                Content = dynamicViewSelector,
                Title = nameof(DynamicViewSelector_Usage)
            };
            window.ShowDialog();
        }
    }
}
