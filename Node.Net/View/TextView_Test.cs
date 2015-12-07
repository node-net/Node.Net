

namespace Node.Net.View
{
    [NUnit.Framework.TestFixture]
    class TextView_Test
    {
        [NUnit.Framework.TestCase,NUnit.Framework.RequiresSTA,NUnit.Framework.Explicit]
        public void TextView_Usage()
        {
            System.Collections.Generic.List<string> doc = new System.Collections.Generic.List<string>();
            doc.Add("The quick brown fox");
            doc.Add("jumps over the lazy dog.");
            TextView textView = new TextView(doc);
            Node.Net.View.Window.ShowDialog(textView, "TextView.Test");
        }
    }
}
