using NUnit.Framework;

namespace Node.Net.View
{
    [TestFixture]
    class Base_Test
    {
        [TestCase,NUnit.Framework.Apartment(System.Threading.ApartmentState.STA),NUnit.Framework.Explicit]
        public void DocumentView_Usage()
        {
            System.Collections.Generic.Dictionary<string, object> doc
                = new System.Collections.Generic.Dictionary<string, object>();
            Base view = new Base(doc);
            System.Windows.Window window = new System.Windows.Window() { Content = view, Title = "DocumentView" };
            window.ShowDialog();
        }
    }
}
