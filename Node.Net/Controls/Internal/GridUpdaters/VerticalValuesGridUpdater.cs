using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Node.Net.Controls.Internal.GridUpdaters
{
    class VerticalValuesGridUpdater : ValuesGridUpdater, IGridUpdater
    {
        public VerticalValuesGridUpdater(System.Windows.Controls.Grid grid) : base(grid) { }

        public override void UpdateGrid(System.Windows.Controls.Grid grid)
        {
            grid.ColumnDefinitions.Clear();
            grid.RowDefinitions.Clear();
            grid.Children.Clear();

            if (grid.DataContext == null) return;
            var dictionary = grid.DataContext.GetValue() as IDictionary;
            if (dictionary != null)
            {
                var keys = new List<string>(GetKeys(dictionary));
                if (keys.Count > 0)
                {
                    var rowIndex = 0;
                    grid.ColumnDefinitions.Add(new ColumnDefinition());
                    if (ShowKeys) grid.ColumnDefinitions.Add(new ColumnDefinition());
                    foreach (string key in keys)
                    {
                        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                        if (ShowKeys)
                        {
                            var keyLabel = new Label { Content = key, Background = Brushes.LightGray };
                            grid.Children.Add(keyLabel);
                            Grid.SetRow(keyLabel, rowIndex);
                        }
                        var keyValue = new Label { Content = dictionary[key].ToString() };
                        grid.Children.Add(keyValue);
                        Grid.SetColumn(keyValue, ShowKeys ? 1 : 0);
                        Grid.SetRow(keyValue, rowIndex);
                        ++rowIndex;
                    }
                }
            }
        }


    }
}
