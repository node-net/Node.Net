using NUnit.Framework;

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
            System.Windows.Window window = new System.Windows.Window();
            window.Content = hashGrid;
            window.Title = "HashGrid_Usage";
            window.ShowDialog();
        }
        
    }
}