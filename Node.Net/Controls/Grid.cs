namespace Node.Net.Controls
{
    public class Grid : System.Windows.Controls.Grid
    {
        private Internal.GridUpdaters.GridUpdater _updater;
        public Grid() { _updater = new Internal.GridUpdaters.GridUpdater(this); }

        public bool ShowRowLabels
        {
            get { return _updater.ShowRowLabels; }
            set { _updater.ShowRowLabels = value;  _updater.UpdateGrid(this); }
        }
    }
}
