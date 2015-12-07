

namespace Node.Net.View
{
    [NUnit.Framework.TestFixture,NUnit.Framework.RequiresSTA]
    class ListViewItem_Test
    {
        [NUnit.Framework.TestCase,NUnit.Framework.Explicit]
        public void ListViewItem_Usage()
        {
            ListViewItem lvItem = new ListViewItem("apple");
            NUnit.Framework.Assert.AreEqual("apple", lvItem.Content.ToString());

            lvItem = new ListViewItem(new System.Collections.Generic.KeyValuePair<string, string>("apple", "Apple"));
            NUnit.Framework.Assert.AreEqual("apple", lvItem.Content.ToString());

            lvItem.Update += lvItem_Update;
            lvItem.DataContext = "banana";
            NUnit.Framework.Assert.AreSame(typeof(System.Windows.Controls.Label), lvItem.Content.GetType());
        }

        void lvItem_Update(object sender, System.EventArgs e)
        {
            ListViewItem lvi = sender as ListViewItem;
            System.Windows.Controls.Label label = new System.Windows.Controls.Label();
            lvi.Content = label;
        }
    }
}
