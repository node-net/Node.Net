using NUnit.Framework;

namespace Node.Net.View
{
    [TestFixture]
    public class ComboBox_Test
    {
        [TestCase,NUnit.Framework.Apartment(System.Threading.ApartmentState.STA), NUnit.Framework.Explicit]
        public void ComboBox_Usage()
        {
            System.Collections.Generic.List<string> fruit = new System.Collections.Generic.List<string>();
            fruit.Add("apple");
            fruit.Add("banana");
            fruit.Add("grape");

            ComboBox comboBox = new ComboBox();
            comboBox.DataContext = fruit;
            System.Windows.Window window = new System.Windows.Window();
            window.Content = comboBox;
            window.Title = "ComboBox_Usage";
            window.ShowDialog();
        }
        [TestCase, NUnit.Framework.Apartment(System.Threading.ApartmentState.STA), NUnit.Framework.Explicit]
        public void ComboBox_Usage2()
        {
            System.Collections.Generic.Dictionary<string,object> fruit = new System.Collections.Generic.Dictionary<string,object>();
            fruit.Add("apple","Apple");
            fruit.Add("banana","Banana");
            fruit.Add("grape","Grape");

            ComboBox comboBox = new ComboBox();
            comboBox.DataContext = fruit;
            System.Windows.Window window = new System.Windows.Window();
            window.Content = comboBox;
            window.Title = "ComboBox_Usage";
            window.ShowDialog();
        }
    }
}
