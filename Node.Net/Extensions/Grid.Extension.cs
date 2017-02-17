using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Node.Net
{
    public static class GridExtension
    {
        public static void AddRow(this Grid grid, string[] content, Brush backgroundBrush = null, Brush foregroundBrush = null)
        {
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            var column = 0;
            foreach (string value in content)
            {
                var label = new Label { Content = value };
                if (backgroundBrush != null) label.Background = backgroundBrush;
                if (foregroundBrush != null) label.Foreground = foregroundBrush;

                grid.Children.Add(label);
                Grid.SetRow(label, grid.RowDefinitions.Count - 1);
                if (grid.ColumnDefinitions.Count < column + 1) grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                Grid.SetColumn(label, column);
                ++column;
            }
        }
    }
}
