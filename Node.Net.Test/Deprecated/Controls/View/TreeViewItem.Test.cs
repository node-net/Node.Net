using NUnit.Framework;

namespace Node.Net.View
{
    [TestFixture,Category("Node.Net.View.TreeViewItem")]
    class TreeViewItem_Test
    {
        [TestCase]
        public void TreeViewItem_IsKeyValuePair()
        {
            Assert.False(KeyValuePair.IsKeyValuePair("abc"));
            Assert.True(KeyValuePair.IsKeyValuePair(new System.Collections.Generic.KeyValuePair<string, dynamic>("string", "abc")));
        }

        [TestCase]
        public void TreeViewItem_GetValue()
        {
            const string s = "abc";
            Assert.AreSame(s, KeyValuePair.GetValue(s));

            var kvp
                = new System.Collections.Generic.KeyValuePair<string, dynamic>("string", "abc");
            NUnit.Framework.Assert.AreSame(kvp.Value, KeyValuePair.GetValue(kvp));
        }
        [TestCase]
        public void TreeViewItem_IsValidChild()
        {
            Assert.False(TreeViewItem.IsValidChild("abc"));
            Assert.False(TreeViewItem.IsValidChild(new System.Collections.Generic.KeyValuePair<string, dynamic>("string", "abc")));
            Assert.False(TreeViewItem.IsValidChild(1.23));
            var dictionary
                = new System.Collections.Generic.Dictionary<string, dynamic>();
            Assert.True(TreeViewItem.IsValidChild(dictionary));
            Assert.True(TreeViewItem.IsValidChild(new System.Collections.Generic.KeyValuePair<string,dynamic>("child",dictionary)));
        }
       
        [TestCase,Apartment(System.Threading.ApartmentState.STA)]
        public void TreeViewItem_Children()
        {
            Assert.AreEqual(0, TreeViewItem.GetChildren("abc").Count);
            Assert.AreEqual(0, TreeViewItem.GetChildren(1.23).Count);
            Assert.AreEqual(0, TreeViewItem.GetChildren(false).Count);
            Assert.AreEqual(0, TreeViewItem.GetChildren(null).Count);

            Assert.AreEqual(0, TreeViewItem.GetChildren(new System.Collections.Generic.KeyValuePair<string,dynamic>("string","abc")).Count);
            Assert.AreEqual(0, TreeViewItem.GetChildren(new System.Collections.Generic.KeyValuePair<string, dynamic>("double", 1.23)).Count);
            Assert.AreEqual(0, TreeViewItem.GetChildren(new System.Collections.Generic.KeyValuePair<string, dynamic>("bool", false)).Count);
            Assert.AreEqual(0, TreeViewItem.GetChildren(new System.Collections.Generic.KeyValuePair<string, dynamic>("null", null)).Count);

            var dictionary
               = new System.Collections.Generic.Dictionary<string, dynamic>();
            Assert.AreEqual(0, TreeViewItem.GetChildren(dictionary).Count);
            dictionary.Add("string", "abc");
            Assert.AreEqual(0, TreeViewItem.GetChildren(dictionary).Count);

            var child
               = new System.Collections.Generic.Dictionary<string, dynamic>();
            dictionary.Add(nameof(child), child);
            Assert.AreEqual(1, TreeViewItem.GetChildren(dictionary).Count);

            var hash = new System.Collections.Generic.Dictionary<string, dynamic>();
            var childHash = new System.Collections.Generic.Dictionary<string, dynamic>();
            hash[nameof(childHash)] = childHash;
            hash["childArray"] = new System.Collections.Generic.List<dynamic>();
            var tvi = new TreeViewItem(hash);
            var children = tvi.GetChildren();
            Assert.AreEqual(2, children.Count);
            Assert.True(KeyValuePair.IsKeyValuePair(children[0]));
            Assert.AreEqual(nameof(childHash), KeyValuePair.GetKey(children[0]));
            Assert.AreSame(childHash, KeyValuePair.GetValue(children[0]));
            Assert.AreEqual("childArray", KeyValuePair.GetKey(children[1]));
        }
        [TestCase,NUnit.Framework.Apartment(System.Threading.ApartmentState.STA), NUnit.Framework.Explicit]
        public void TreeViewItem_Usage()
        {
            var dictionary
                = new System.Collections.Generic.Dictionary<string, dynamic>();
            var dictionaryA
                = new System.Collections.Generic.Dictionary<string, dynamic>();
            var dictionaryA1
                = new System.Collections.Generic.Dictionary<string, dynamic>();
            dictionaryA.Add(nameof(dictionaryA1), dictionaryA1);
            dictionary.Add(nameof(dictionaryA), dictionaryA);
            dictionary.Add("string", "abc");

            var tvi = new TreeViewItem("Root", dictionary, 3);
            Assert.AreSame(tvi.DataContext, dictionary);
            Assert.AreEqual(1, tvi.Items.Count);
            var tvi2 = tvi.Items[0] as TreeViewItem;

            var treeView = new System.Windows.Controls.TreeView();
            treeView.Items.Add(tvi);

            var window = new System.Windows.Window { Content = treeView, Title= nameof(TreeViewItem_Usage) };
            window.ShowDialog();
        }
    }
}
