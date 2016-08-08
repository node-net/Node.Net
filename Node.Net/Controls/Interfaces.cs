namespace Node.Net.Controls
{
    public interface IFactory { T Create<T>(object value); }
    public interface IGridUpdater { void UpdateGrid(System.Windows.Controls.Grid grid); }
}
