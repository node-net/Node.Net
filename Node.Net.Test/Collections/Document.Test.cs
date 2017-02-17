using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Node.Net.Collections
{
    [TestFixture]
    class DocumentTest
    {
        [Test]
        public void Document_Open()
        {
            var document = Open("States.json");
            foreach(var item in document.ItemsSource)
            {
                var dictionary = item as Dictionary;
                Assert.NotNull(dictionary);
                Assert.AreNotEqual(0, dictionary.ItemsSource.Count);
            }
        }
        [Test,Explicit,Apartment(ApartmentState.STA)]
        public void Document_TreeView_ShowDialog()
        {
            var document = Open("States.json");
            Assert.NotNull(document, nameof(document));
            var tree = new Test.TreeViewTest { DataContext = document };
           
            new Window
            {
                Title = nameof(Document_TreeView_ShowDialog),
                WindowState = WindowState.Maximized,
                Content = tree
            }.ShowDialog();
        }

        public static Document Open(string name)
        {
            foreach(string resource_name in typeof(DocumentTest).Assembly.GetManifestResourceNames())
            {
                if(resource_name.Contains(name))
                {
                    return Node.Net.Readers.JsonReader.Default.Read(typeof(DocumentTest).Assembly.GetManifestResourceStream(resource_name)) as Document;
                }
            }
            throw new System.InvalidOperationException($"unable to Open {name}");
        }

        [Test]
        public void Document_Usage()
        {
            var doc = Open("States.json");
            doc.DeepUpdateParents();
            Assert.NotNull(doc, nameof(doc));

            foreach (var key in doc.Keys)
            {
                var child = doc[key] as IDictionary;
                if (child != null)
                {
                    var parent = child.GetParent() as IDictionary;
                    Assert.NotNull(parent, nameof(parent));

                    var name = child.GetName();
                    Assert.AreEqual(key, name);
                    Assert.True(child.GetName().Length > 0);
                }
            }
        }
    }
}
