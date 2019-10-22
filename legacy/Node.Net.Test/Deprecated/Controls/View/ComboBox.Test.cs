using NUnit.Framework;

namespace Node.Net.View
{
    [TestFixture]
    public class ComboBox_Test
    {
        [TestCase,NUnit.Framework.Apartment(System.Threading.ApartmentState.STA), NUnit.Framework.Explicit]
        public void ComboBox_Usage()
        {
            var fruit = new System.Collections.Generic.List<string>();
            fruit.Add("apple");
            fruit.Add("banana");
            fruit.Add("grape");

            var comboBox = new ComboBox
            {
                DataContext = fruit
            };
            var window = new System.Windows.Window
            {
                Content = comboBox,
                Title = nameof(ComboBox_Usage)
            };
            window.ShowDialog();
        }
        [TestCase, NUnit.Framework.Apartment(System.Threading.ApartmentState.STA), NUnit.Framework.Explicit]
        public void ComboBox_Usage2()
        {
            var fruit = new System.Collections.Generic.Dictionary<string, object>();
            fruit.Add("apple", "Apple");
            fruit.Add("banana", "Banana");
            fruit.Add("grape", "Grape");

            var comboBox = new ComboBox
            {
                DataContext = fruit
            };
            var window = new System.Windows.Window
            {
                Content = comboBox,
                Title = nameof(ComboBox_Usage)
            };
            window.ShowDialog();
        }
    }
}
