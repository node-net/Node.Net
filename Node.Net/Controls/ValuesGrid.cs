using System.Windows.Controls;

namespace Node.Net.Controls
{
    class ValuesGrid : System.Windows.Controls.Grid
    {

        public ValuesGrid() { _iGridUpdater = new Internal.GridUpdaters.VerticalValuesGridUpdater(this); }
        public Orientation Orientation
        {
            get { return _orientation; }
            set
            {
                _orientation = value;
                if (_orientation == Orientation.Horizontal) _iGridUpdater = new Internal.GridUpdaters.HorizontalValuesGridUpdater(this);
                else _iGridUpdater = new Internal.GridUpdaters.VerticalValuesGridUpdater(this);
            }
        }
        private IGridUpdater _iGridUpdater;
        private Orientation _orientation = Orientation.Vertical;
    }
}
