using System;
using System.Windows;

namespace Node.Net.Controls
{
    public interface IDataContext { object DataContext { get; set; } }
    public interface IFactory { T Create<T>(object value); }
    public interface IGridUpdater { void UpdateGrid(System.Windows.Controls.Grid grid); }
    public interface ISelectedItem { object SelectedItem { get; } }
    public interface ISelectedItemChanged : ISelectedItem,IDataContext { event RoutedPropertyChangedEventHandler<object> SelectedItemChanged; }

}
