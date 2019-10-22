using NUnit.Framework;

namespace Node.Net.View
{
    [TestFixture,NUnit.Framework.Apartment(System.Threading.ApartmentState.STA)]
    class ListViewItem_Test
    {
        [TestCase,NUnit.Framework.Explicit]
        public void ListViewItem_Usage()
        {
            var lvItem = new ListViewItem("apple");
            Assert.AreEqual("apple", lvItem.Content.ToString());

            lvItem = new ListViewItem(new System.Collections.Generic.KeyValuePair<string, string>("apple", "Apple"));
            Assert.AreEqual("apple", lvItem.Content.ToString());

            lvItem.Update += lvItem_Update;
            lvItem.DataContext = "banana";
            Assert.AreSame(typeof(System.Windows.Controls.Label), lvItem.Content.GetType());
        }

        static void lvItem_Update(object sender, System.EventArgs e)
        {
            var lvi = sender as ListViewItem;
            var label = new System.Windows.Controls.Label();
            lvi.Content = label;
        }
    }
}
