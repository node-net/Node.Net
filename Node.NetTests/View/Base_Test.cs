

namespace Node.Net.View
{
    [NUnit.Framework.TestFixture]
    class Base_Test
    {
        [NUnit.Framework.TestCase,NUnit.Framework.RequiresSTA,NUnit.Framework.Explicit]
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
