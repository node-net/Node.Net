using NUnit.Framework;

namespace Node.Net.View
{
    [TestFixture]
    class DynamicViewSelector_Test
    {
        [TestCase,NUnit.Framework.Apartment(System.Threading.ApartmentState.STA),NUnit.Framework.Explicit]
        public void DynamicViewSelector_Usage()
        {
            DynamicViewSelector dynamicViewSelector = new DynamicViewSelector(new Properties());
            dynamicViewSelector.DynamicView.Elements.Add("ListView", new ListView());
            dynamicViewSelector.DynamicView.Elements.Add("TreeView", new TreeView());
            dynamicViewSelector.DynamicView.Elements.Add("TextView", new TextView());
            dynamicViewSelector.DataContext = new Widget();
            System.Windows.Window window = new System.Windows.Window();
            window.Content = dynamicViewSelector;
            window.Title = "DynamicViewSelector_Usage";
            window.ShowDialog();
        }
    }
}
