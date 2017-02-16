
using NUnit.Framework;
namespace Node.Net.View
{
    [TestFixture]
    class TextView_Test
    {
        [TestCase, Apartment(System.Threading.ApartmentState.STA), NUnit.Framework.Explicit]
        public void TextView_Usage()
        {
            var doc = new System.Collections.Generic.List<string>();
            doc.Add("The quick brown fox");
            doc.Add("jumps over the lazy dog.");
            var textView = new TextView(doc);
            Node.Net.View.Window.ShowDialog(textView, "TextView.Test");
        }
    }
}
