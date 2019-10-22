using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using static System.Environment;

namespace Node.Net.Deprecated.Controls.TreeViewItems
{
    [TestFixture, Category("Controls")]
    class TreeViewItemTest
    {
        [Test, Explicit, Apartment(ApartmentState.STA)]
        public void IDictionaryTreeViewItem_Usage()
        {
            ViewTester.ShowDialog(GetModels(), new IDictionaryTreeViewItem());
        }

        public object GetModels()
        {
            var models = new Dictionary<string, dynamic>();
            foreach (var json_file in Directory.GetFiles($"{GetEnvironmentVariable("DEV_ROOT")}\\data", "*.json"))
            {
                using (FileStream stream = new FileStream(json_file, FileMode.Open))
                {
                    models.Add(json_file, new KeyValuePair<string, dynamic>(json_file, Node.Net.Deprecated.Json.Reader.Default.Load(stream, json_file)));
                }
            }
            return models;
        }
    }

}
