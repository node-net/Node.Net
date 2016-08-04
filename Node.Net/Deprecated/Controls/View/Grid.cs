namespace Node.Net.View
{
    public class Grid : System.Windows.Controls.Grid
    {
        public Grid()
        {
            DataContextChanged += Grid_DataContextChanged;
        }

        void Grid_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            Update();
        }

        private void Update()
        {
            ColumnDefinitions.Clear();
            RowDefinitions.Clear();
            Children.Clear();

            var array = KeyValuePair.GetValue(DataContext) as System.Array;
            UpdateArray(array);

            var dictionary = KeyValuePair.GetValue(DataContext) as System.Collections.IDictionary;
            if (!ReferenceEquals(null,dictionary))
            {
                UpdateDictionary(dictionary);
            }
        }
        private void UpdateArray(System.Array array)
        {
            if (!ReferenceEquals(null, array) && array.Rank == 2)
            {
                var ncols = array.GetLength(0);
                var nrows = array.GetLength(1);
                for (int c = 0; c < ncols; ++c)
                {
                    ColumnDefinitions.Add(new System.Windows.Controls.ColumnDefinition());
                }
                for (int r = 0; r < nrows; ++r)
                {
                    RowDefinitions.Add(new System.Windows.Controls.RowDefinition());
                }
                for (int c = 0; c < ncols; ++c)
                {
                    for (int r = 0; r < nrows; ++r)
                    {
                        var item = array.GetValue(c, r);
                        if (!ReferenceEquals(null, item))
                        {
                            var value = array.GetValue(c, r).ToString();
                            var label
                                = new System.Windows.Controls.Label
                                {
                                    Content = value,
                                    HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                                    VerticalAlignment = System.Windows.VerticalAlignment.Center
                                };
                            Children.Add(label);
                            System.Windows.Controls.Grid.SetColumn(label, c);
                            System.Windows.Controls.Grid.SetRow(label, r);
                        }
                    }
                }
            }
        }

        private static void UpdateDictionary(System.Collections.IDictionary dictionary)
        {
            if(!ReferenceEquals(null,dictionary))
            {
                foreach (object key in dictionary.Keys)
                {
                    var value = dictionary[key];
                    var ienumerable = value as System.Collections.IEnumerable;
                    if (!ReferenceEquals(null,value) && value.GetType() != typeof(string) && !ReferenceEquals(null,ienumerable))
                    {
                        foreach(object item in ienumerable)
                        {
                            if(!ReferenceEquals(null,item))
                            {
                                if(item.GetType().IsValueType || item.GetType() == typeof(string))
                                {

                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
