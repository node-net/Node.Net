using NUnit.Framework;

namespace Node.Net.View
{
    class Car
    {
        public Car() { }
        public Car(string make, string model) { Make = make; Model = model; }

        public string Make { get; set; } = "Toyota";

        public string Model { get; set; } = "Prius";
    }
    [TestFixture]
    class DataGridView_Test
    {
        [TestCase,NUnit.Framework.Apartment(System.Threading.ApartmentState.STA),NUnit.Framework.Explicit]
        public void DataGridView_Usage()
        {
            var dataGridView = new DataGridView();
            Car[] cars = { new Car(), new Car("Ford", "Focus") };
            dataGridView.DataContext = cars;
            var window = new System.Windows.Window
            {
                Content = dataGridView,
                Title = nameof(DataGridView_Usage)
            };
            window.ShowDialog();
        }
    }
}
