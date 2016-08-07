using System;

namespace Node.Net.Controls
{
    class TreeView : System.Windows.Controls.TreeView
    {
        private Internal.TreeViewUpdater _updater;
        public TreeView()
        {
            HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            HorizontalContentAlignment = System.Windows.HorizontalAlignment.Stretch;
            _updater = new Internal.TreeViewUpdater(this);
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            _updater.UpdateTreeView(this);
        }
    }
}
