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

            System.Array array = KeyValuePair.GetValue(DataContext) as System.Array;
            UpdateArray(array);

            System.Collections.IDictionary dictionary = KeyValuePair.GetValue(DataContext) as System.Collections.IDictionary;
            if(!object.ReferenceEquals(null,dictionary))
            {
                UpdateDictionary(dictionary);
            }
        }
        private void UpdateArray(System.Array array)
        {
            if (!object.ReferenceEquals(null, array) && array.Rank == 2)
            {
                int ncols = array.GetLength(0);
                int nrows = array.GetLength(1);
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
                        object item = array.GetValue(c, r);
                        if (!object.ReferenceEquals(null, item))
                        {
                            string value = array.GetValue(c, r).ToString();
                            System.Windows.Controls.Label label
                                = new System.Windows.Controls.Label()
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

        private void UpdateDictionary(System.Collections.IDictionary dictionary)
        {
            if(!object.ReferenceEquals(null,dictionary))
            {
                foreach (object key in dictionary.Keys)
                {
                    object value = dictionary[key];
                    System.Collections.IEnumerable ienumerable = value as System.Collections.IEnumerable;
                    if(!object.ReferenceEquals(null,value) && value.GetType() != typeof(string) && !object.ReferenceEquals(null,ienumerable))
                    {
                        foreach(object item in ienumerable)
                        {
                            if(!object.ReferenceEquals(null,item))
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
