

namespace Node.Net.View
{
    [NUnit.Framework.TestFixture]
    class TreeViewItem_Test
    {
        [NUnit.Framework.TestCase]
        public void TreeViewItem_IsKeyValuePair()
        {
            NUnit.Framework.Assert.False(KeyValuePair.IsKeyValuePair("abc"));
            NUnit.Framework.Assert.True(KeyValuePair.IsKeyValuePair(new System.Collections.Generic.KeyValuePair<string, dynamic>("string", "abc")));
        }

        [NUnit.Framework.TestCase]
        public void TreeViewItem_GetValue()
        {
            string s = "abc";
            NUnit.Framework.Assert.AreSame(s, KeyValuePair.GetValue(s));

            System.Collections.Generic.KeyValuePair<string, dynamic> kvp
                = new System.Collections.Generic.KeyValuePair<string, dynamic>("string", "abc");
            NUnit.Framework.Assert.AreSame(kvp.Value, KeyValuePair.GetValue(kvp));
        }
        [NUnit.Framework.TestCase]
        public void TreeViewItem_IsValidChild()
        {
            NUnit.Framework.Assert.False(TreeViewItem.IsValidChild("abc"));
            NUnit.Framework.Assert.False(TreeViewItem.IsValidChild(new System.Collections.Generic.KeyValuePair<string, dynamic>("string", "abc")));
            NUnit.Framework.Assert.False(TreeViewItem.IsValidChild(1.23));
            System.Collections.Generic.Dictionary<string, dynamic> dictionary
                = new System.Collections.Generic.Dictionary<string, dynamic>();
            NUnit.Framework.Assert.True(TreeViewItem.IsValidChild(dictionary));
            NUnit.Framework.Assert.True(TreeViewItem.IsValidChild(new System.Collections.Generic.KeyValuePair<string,dynamic>("child",dictionary)));
        }
       
        [NUnit.Framework.TestCase,NUnit.Framework.RequiresSTA]
        public void TreeViewItem_Children()
        {
            NUnit.Framework.Assert.AreEqual(0, TreeViewItem.GetChildren("abc").Count);
            NUnit.Framework.Assert.AreEqual(0, TreeViewItem.GetChildren(1.23).Count);
            NUnit.Framework.Assert.AreEqual(0, TreeViewItem.GetChildren(false).Count);
            NUnit.Framework.Assert.AreEqual(0, TreeViewItem.GetChildren(null).Count);

            NUnit.Framework.Assert.AreEqual(0, TreeViewItem.GetChildren(new System.Collections.Generic.KeyValuePair<string,dynamic>("string","abc")).Count);
            NUnit.Framework.Assert.AreEqual(0, TreeViewItem.GetChildren(new System.Collections.Generic.KeyValuePair<string, dynamic>("double", 1.23)).Count);
            NUnit.Framework.Assert.AreEqual(0, TreeViewItem.GetChildren(new System.Collections.Generic.KeyValuePair<string, dynamic>("bool", false)).Count);
            NUnit.Framework.Assert.AreEqual(0, TreeViewItem.GetChildren(new System.Collections.Generic.KeyValuePair<string, dynamic>("null", null)).Count);

            System.Collections.Generic.Dictionary<string, dynamic> dictionary
               = new System.Collections.Generic.Dictionary<string, dynamic>();
            NUnit.Framework.Assert.AreEqual(0, TreeViewItem.GetChildren(dictionary).Count);
            dictionary.Add("string", "abc");
            NUnit.Framework.Assert.AreEqual(0, TreeViewItem.GetChildren(dictionary).Count);

            System.Collections.Generic.Dictionary<string, dynamic> child
               = new System.Collections.Generic.Dictionary<string, dynamic>();
            dictionary.Add("child", child);
            NUnit.Framework.Assert.AreEqual(1, TreeViewItem.GetChildren(dictionary).Count);

            System.Collections.Generic.Dictionary<string, dynamic> hash = new System.Collections.Generic.Dictionary<string, dynamic>();
            System.Collections.Generic.Dictionary<string,dynamic> childHash = new System.Collections.Generic.Dictionary<string, dynamic>();
            hash["childHash"] = childHash;
            hash["childArray"] = new System.Collections.Generic.List<dynamic>();
            TreeViewItem tvi = new TreeViewItem(hash);
            System.Collections.IList children = tvi.GetChildren();
            NUnit.Framework.Assert.AreEqual(2, children.Count);
            NUnit.Framework.Assert.True(KeyValuePair.IsKeyValuePair(children[0]));
            NUnit.Framework.Assert.AreEqual("childHash", KeyValuePair.GetKey(children[0]));
            NUnit.Framework.Assert.AreSame(childHash, KeyValuePair.GetValue(children[0]));
            NUnit.Framework.Assert.AreEqual("childArray", KeyValuePair.GetKey(children[1]));
        }
        [NUnit.Framework.TestCase,NUnit.Framework.RequiresSTA,NUnit.Framework.Explicit]
        public void TreeViewItem_Usage()
        {
            System.Collections.Generic.Dictionary<string, dynamic> dictionary
                = new System.Collections.Generic.Dictionary<string, dynamic>();
            System.Collections.Generic.Dictionary<string, dynamic> dictionaryA
                = new System.Collections.Generic.Dictionary<string, dynamic>();
            System.Collections.Generic.Dictionary<string, dynamic> dictionaryA1
                = new System.Collections.Generic.Dictionary<string, dynamic>();
            dictionaryA.Add("dictionaryA1", dictionaryA1);
            dictionary.Add("dictionaryA", dictionaryA);
            dictionary.Add("string", "abc");

            TreeViewItem tvi = new TreeViewItem("Root", dictionary, 3);
            NUnit.Framework.Assert.AreSame(tvi.DataContext, dictionary);
            NUnit.Framework.Assert.AreEqual(1, tvi.Items.Count);
            TreeViewItem tvi2 = tvi.Items[0] as TreeViewItem;
            
            System.Windows.Controls.TreeView treeView = new System.Windows.Controls.TreeView();
            treeView.Items.Add(tvi);

            System.Windows.Window window = new System.Windows.Window() { Content = treeView, Title="TreeViewItem_Usage" };
            window.ShowDialog();
        }
    }
}
