using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Node.Net.Deprecated.Controls.Internal.GridUpdaters
{
    class GridUpdater : IGridUpdater
    {
        public GridUpdater(Grid grid)
        {
            grid.DataContextChanged += Grid_DataContextChanged;
        }

        private bool showColumnLabels = true;
        public bool ShowColumnLabels
        {
            get { return showColumnLabels; }
            set { showColumnLabels = value; }
        }

        private bool showRowLabels = true;
        public bool ShowRowLabels
        {
            get { return showRowLabels; }
            set { showRowLabels = value; }
        }

        public GridLength DefaultColumnWidth { get; set; } = new GridLength(1, GridUnitType.Star);

        private void Grid_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            UpdateGrid(sender as Grid);
        }

        public void UpdateGrid(System.Windows.Controls.Grid grid)
        {
            grid.ColumnDefinitions.Clear();
            grid.RowDefinitions.Clear();
            grid.Children.Clear();

            var dictionary = grid.DataContext.GetValue() as IDictionary;
            if (dictionary != null)
            {
                var rowKeys = GetRowKeys(dictionary);
                var columnKeys = GetColumnKeys(dictionary);
                if (rowKeys.Length > 0 && columnKeys.Length > 0)
                {
                    var colIndex = 0;
                    var rowIndex = 0;

                    while (grid.RowDefinitions.Count < rowKeys.Length) grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                    while (grid.ColumnDefinitions.Count < columnKeys.Length) grid.ColumnDefinitions.Add(new ColumnDefinition { Width = DefaultColumnWidth });
                    if (ShowColumnLabels) grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); 
                    if (ShowColumnLabels)
                    {
                        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = DefaultColumnWidth });
                        colIndex = ShowRowLabels? 1 : 0;
                        foreach (string columnKey in columnKeys)
                        {
                            var columnLabel = new Label { Content = columnKey, Background = Brushes.LightGray };
                            grid.Children.Add(columnLabel);
                            Grid.SetColumn(columnLabel, colIndex);
                            ++colIndex;
                        }
                        ++rowIndex;
                    }
                    foreach (var rowKey in rowKeys)
                    {
                        var rowDictionary = dictionary[rowKey] as IDictionary;
                        if (ShowRowLabels)
                        {
                            var rowLabel = new Label { Content = rowKey, Background = Brushes.LightGray };
                            grid.Children.Add(rowLabel);
                            Grid.SetRow(rowLabel, rowIndex);
                        }
                        colIndex = ShowRowLabels ? 1 : 0;
                        foreach (string columnKey in columnKeys)
                        {
                            if (rowDictionary.Contains(columnKey))
                            {
                                var value = rowDictionary[columnKey];
                                if (value != null)
                                {
                                    var valueControl = new Label { Content = value.ToString() };
                                    grid.Children.Add(valueControl);
                                    Grid.SetRow(valueControl, rowIndex);
                                    Grid.SetColumn(valueControl, colIndex);
                                }

                            }
                            ++colIndex;
                        }
                        ++rowIndex;
                    }
                }
            }
        }

        private string[] GetRowKeys(IDictionary dictionary)
        {
            var keys = new List<string>();
            foreach (string key in dictionary.Keys)
            {
                var value = dictionary[key] as IDictionary;
                if (value != null) keys.Add(key);
            }
            return keys.ToArray();
        }
        private string[] GetColumnKeys(IDictionary dictionary)
        {
            var keys = new List<string>();
            foreach (string rowKey in dictionary.Keys)
            {
                var childDictionary = dictionary[rowKey] as IDictionary;
                if (childDictionary != null)
                {
                    foreach (string columnKey in childDictionary.Keys)
                    {
                        var value = childDictionary[columnKey];
                        if (value == null || value.GetType().IsValueType || value.GetType() == typeof(string))
                        {
                            if (!keys.Contains(columnKey)) keys.Add(columnKey);
                        }
                    }
                }
            }
            return keys.ToArray();
        }
    }
}
