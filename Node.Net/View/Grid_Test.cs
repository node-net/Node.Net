using NUnit.Framework;
namespace Node.Net.View
{
    [TestFixture]
    class Grid_Test
    {
        [TestCase,NUnit.Framework.Apartment(System.Threading.ApartmentState.STA),NUnit.Framework.Explicit]
        public void Grid_2D_String_Array()
        {
            string[,] data = new string[5,6];
            for(int i = 0; i < 5; ++i)
            {
                for(int j = 0; j < 6; ++j)
                {
                    data[i, j] = i.ToString() + "," + j.ToString();
                }
            }
            Grid grid = new Grid() { DataContext = data };
            System.Windows.Window window = new System.Windows.Window();
            window.Content = grid;
            window.Title = "Grid_2D_String_Array";
            window.ShowDialog();
        }

        [TestCase,NUnit.Framework.Apartment(System.Threading.ApartmentState.STA),NUnit.Framework.Explicit]
        public void Grid_2D_String_Array_With_Nulls()
        {
            string[,] data = new string[5, 6];
            for (int i = 0; i < 5; ++i)
            {
                for (int j = 0; j < 3; ++j)
                {
                    data[i, j] = i.ToString() + "," + j.ToString();
                }
            }
            Grid grid = new Grid() { DataContext = data };
            System.Windows.Window window = new System.Windows.Window();
            window.Content = grid;
            window.Title = "Grid_2D_String_Array_With_Nulls";
            window.ShowDialog();
        }

        [TestCase,NUnit.Framework.Apartment(System.Threading.ApartmentState.STA),NUnit.Framework.Explicit]
        public void Grid_Dictionary()
        {
            System.Collections.Generic.Dictionary<string, string[]> dictionary
                = new System.Collections.Generic.Dictionary<string, string[]>();

            System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
            list.Add("X (ft)");
            list.Add("Y (ft)");
            list.Add("Value (lbs)");
            dictionary.Add("header", list.ToArray());
            Grid grid = new Grid() { DataContext = dictionary };
            System.Windows.Window window = new System.Windows.Window();
            window.Content = grid;
            window.Title = "Grid_Dictionary";
            window.ShowDialog();
        }

        [TestCase,NUnit.Framework.Apartment(System.Threading.ApartmentState.STA),NUnit.Framework.Explicit]
        public void Grid_2D_Double_Array()
        {
            double[,] data = new double[5, 6];
            for (int i = 0; i < 5; ++i)
            {
                for (int j = 0; j < 6; ++j)
                {
                    data[i, j] = i * 10 + j;
                }
            }
            Grid grid = new Grid() { DataContext = data };
            System.Windows.Window window = new System.Windows.Window();
            window.Content = grid;
            window.Title = "Grid_2D_String_Array";
            window.ShowDialog();
        }
    }
}
