using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Node.Net.Deprecated.Controls.Internal.GridUpdaters
{
    class HorizontalValuesGridUpdater : ValuesGridUpdater, IGridUpdater
    {
        public HorizontalValuesGridUpdater(System.Windows.Controls.Grid grid) : base(grid) { }


        public Orientation Orientation { get; set; } = System.Windows.Controls.Orientation.Horizontal;

        public override void UpdateGrid(System.Windows.Controls.Grid grid)
        {
            if (Orientation == Orientation.Vertical) UpdateGrid_Vertical(grid);
            else UpdateGrid_Horizontal(grid);
        }

        private void UpdateGrid_Horizontal(System.Windows.Controls.Grid grid)
        {
            grid.ColumnDefinitions.Clear();
            grid.RowDefinitions.Clear();
            grid.Children.Clear();

            if (grid.DataContext == null) return;
            var dictionary = Internal.KeyValuePair.GetValue(grid.DataContext) as IDictionary;
            if (dictionary != null)
            {
                var keys = new List<string>(GetKeys(dictionary));
                if (keys.Count > 0)
                {
                    var columnIndex = 0;
                    grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                    foreach (string key in keys)
                    {
                        if (ShowKeys)
                        {
                            grid.ColumnDefinitions.Add(new ColumnDefinition ());
                            var keyElement = GetKeyElement(dictionary, key);
                            grid.Children.Add(keyElement);
                            Grid.SetColumn(keyElement, columnIndex);
                            ++columnIndex;
                        }
                        grid.ColumnDefinitions.Add(new ColumnDefinition());
                        var valueElement = GetValueElement(dictionary, key);
                        grid.Children.Add(valueElement);
                        Grid.SetColumn(valueElement, columnIndex);
                        ++columnIndex;
                    }
                }
            }
        }
        private void UpdateGrid_Vertical(System.Windows.Controls.Grid grid)
        {
            grid.ColumnDefinitions.Clear();
            grid.RowDefinitions.Clear();
            grid.Children.Clear();

            if (grid.DataContext == null) return;
            var dictionary = Internal.KeyValuePair.GetValue(grid.DataContext) as IDictionary;
            if (dictionary != null)
            {
                var keys = new List<string>(GetKeys(dictionary));
                if (keys.Count > 0)
                {
                    var columnIndex = 0;
                    grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                    if (ShowKeys) grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                    foreach (string key in keys)
                    {


                        grid.ColumnDefinitions.Add(new ColumnDefinition());
                        if (ShowKeys)
                        {
                            var keyElement = GetKeyElement(dictionary, key);
                            grid.Children.Add(keyElement);
                            Grid.SetColumn(keyElement, columnIndex);
                        }
                        var valueElement = GetValueElement(dictionary, key);
                        grid.Children.Add(valueElement);
                        Grid.SetRow(valueElement, ShowKeys ? 1 : 0);
                        Grid.SetColumn(valueElement, columnIndex);
                        ++columnIndex;
                    }
                }
            }
        }
    }
}
