using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using static System.Environment;

namespace Node.Net.Controls.Test
{
    [TestFixture]
    class TreeViewTest
    {
        [Test,Explicit,Apartment(ApartmentState.STA)]
        public void TreeView_IDictionary_Usage()
        {
            var assembly = typeof(TreeViewTest).Assembly;
            var imageSource = Factory.Default.Create<ImageSource>(assembly.GetManifestResourceStream("Node.Net.Controls.Test.Resources.Hash.png"));
            Node.Net.Controls.Factories.ImageSourceFactory.Default.AddImageSource("IDictionary", imageSource);
            var d = GlobalFixture.Read("Project.Office.json");
            var w = new Window
            {
                Title = "TreeView_IDictionary_Usage",
                Content = new Node.Net.Controls.TreeView { DataContext = new KeyValuePair<string,dynamic>("Project.Office.json",d) }
            };
            w.ShowDialog();
        }
        [Test, Explicit, Apartment(ApartmentState.STA)]
        public void TreeView_ResourceDictionary_Usage()
        {
            var assembly = typeof(TreeViewTest).Assembly;
            var imageSource = Factory.Default.Create<ImageSource>(assembly.GetManifestResourceStream("Node.Net.Controls.Test.Resources.Hash.png"));
            Node.Net.Controls.Factories.ImageSourceFactory.Default.AddImageSource("IDictionary", imageSource);
            var d = GlobalFixture.Read("ResourceDictionary.Primitives.xaml");
            var w = new Window
            {
                Title = "TreeView_ResourceDictionary_Usage",
                Content = new Node.Net.Controls.TreeView { DataContext = new KeyValuePair<string, dynamic>("ResourceDictionary.Primitives.xaml", d) }
            };
            w.ShowDialog();
        }
        [Test, Explicit, Apartment(ApartmentState.STA)]
        public void TreeView_User_Resources_Usage()
        {
            var assembly = typeof(TreeViewTest).Assembly;
            Node.Net.Controls.Factories.ImageSourceFactory.Default.Import($"{GetFolderPath(SpecialFolder.UserProfile)}\\Resources");
            //var imageSource = Factory.Default.Create<ImageSource>(assembly.GetManifestResourceStream("Node.Net.Controls.Test.Resources.Hash.png"));
            //Node.Net.Controls.Factories.ImageSourceFactory.Default.AddImageSource("IDictionary", imageSource);
            var d = GlobalFixture.Read("Project.Office.json");
            var w = new Window
            {
                Title = "TreeView_User_Resources_Usage",
                Content = new Node.Net.Controls.TreeView { DataContext = new KeyValuePair<string, dynamic>("Project.Office.json", d) }
            };
            w.ShowDialog();
        }
    }
}
