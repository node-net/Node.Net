

namespace Node.Net.View
{
    class Car
    {
        public Car() { }
        public Car(string make, string model) { Make = make; Model = model; }
        string make = "Toyota";
        string model = "Prius";

        public string Make
        {
            get
            {
                return make;
            }
            set { make = value; }
        }

        public string Model
        {
            get { return model; }
            set { model = value; }
        }
    }
    [NUnit.Framework.TestFixture]
    class DataGridView_Test
    {
        [NUnit.Framework.TestCase,NUnit.Framework.RequiresSTA,NUnit.Framework.Explicit]
        public void DataGridView_Usage()
        {
            DataGridView dataGridView = new DataGridView();
            Car[] cars = {new Car(),new Car("Ford","Focus")};
            dataGridView.DataContext = cars;
            System.Windows.Window window = new System.Windows.Window();
            window.Content = dataGridView;
            window.Title = "DataGridView_Usage";
            window.ShowDialog();
        }
    }
}
