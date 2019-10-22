using NUnit.Framework;
namespace Node.Net.View
{
    [TestFixture]
    class Grid_Test
    {
        [TestCase,NUnit.Framework.Apartment(System.Threading.ApartmentState.STA),NUnit.Framework.Explicit]
        public void Grid_2D_String_Array()
        {
            var data = new string[5, 6];
            for (int i = 0; i < 5; ++i)
            {
                for (int j = 0; j < 6; ++j)
                {
                    data[i, j] = i.ToString() + "," + j.ToString();
                }
            }
            var grid = new Grid { DataContext = data };
            var window = new System.Windows.Window
            {
                Content = grid,
                Title = nameof(Grid_2D_String_Array)
            };
            window.ShowDialog();
        }

        [TestCase,NUnit.Framework.Apartment(System.Threading.ApartmentState.STA),NUnit.Framework.Explicit]
        public void Grid_2D_String_Array_With_Nulls()
        {
            var data = new string[5, 6];
            for (int i = 0; i < 5; ++i)
            {
                for (int j = 0; j < 3; ++j)
                {
                    data[i, j] = i.ToString() + "," + j.ToString();
                }
            }
            var grid = new Grid { DataContext = data };
            var window = new System.Windows.Window
            {
                Content = grid,
                Title = nameof(Grid_2D_String_Array_With_Nulls)
            };
            window.ShowDialog();
        }

        [TestCase,NUnit.Framework.Apartment(System.Threading.ApartmentState.STA),NUnit.Framework.Explicit]
        public void Grid_Dictionary()
        {
            var dictionary
       = new System.Collections.Generic.Dictionary<string, string[]>();

            var list = new System.Collections.Generic.List<string>();
            list.Add("X (ft)");
            list.Add("Y (ft)");
            list.Add("Value (lbs)");
            dictionary.Add("header", list.ToArray());
            var grid = new Grid { DataContext = dictionary };
            var window = new System.Windows.Window
            {
                Content = grid,
                Title = nameof(Grid_Dictionary)
            };
            window.ShowDialog();
        }

        [TestCase,NUnit.Framework.Apartment(System.Threading.ApartmentState.STA),NUnit.Framework.Explicit]
        public void Grid_2D_Double_Array()
        {
            var data = new double[5, 6];
            for (int i = 0; i < 5; ++i)
            {
                for (int j = 0; j < 6; ++j)
                {
                    data[i, j] = i * 10 + j;
                }
            }
            var grid = new Grid { DataContext = data };
            var window = new System.Windows.Window
            {
                Content = grid,
                Title = nameof(Grid_2D_String_Array)
            };
            window.ShowDialog();
        }
    }
}
