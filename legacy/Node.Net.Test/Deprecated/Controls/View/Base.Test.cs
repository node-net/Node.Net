using NUnit.Framework;

namespace Node.Net.View
{
    [TestFixture]
    class Base_Test
    {
        [TestCase,NUnit.Framework.Apartment(System.Threading.ApartmentState.STA),NUnit.Framework.Explicit]
        public void DocumentView_Usage()
        {
            var doc
                = new System.Collections.Generic.Dictionary<string, object>();
            var view = new Base(doc);
            var window = new System.Windows.Window { Content = view, Title = "DocumentView" };
            window.ShowDialog();
        }
    }
}
