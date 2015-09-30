using NUnit.Framework;
using System.Windows.Controls;

namespace Node.Net.View
{
    [TestFixture]
    class HashGrid_Test
    {
        [TestCase,RequiresSTA,Explicit,Category("HashGrid")]
        public void HashGrid_Usage()
        {
            Node.Net.Json.Hash hash = new Node.Net.Json.Hash();
            hash["Name"] = "test";
            hash["Length"] = 35.5;
            hash["Age"] = "2 days";
            hash["Null"] = null;

            HashGrid hashGrid = new HashGrid(hash);
            HashGridEditor hashGridEditor = new HashGridEditor(hash);
            StackPanel sp = new StackPanel() { Orientation = Orientation.Horizontal };
            sp.Children.Add(hashGrid);
            sp.Children.Add(hashGridEditor);
            System.Windows.Window window = new System.Windows.Window();
            window.Content = sp;
            window.Title = "HashGrid_Usage";
            window.ShowDialog();
        }
        
    }
}