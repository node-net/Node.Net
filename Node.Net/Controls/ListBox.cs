using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Controls
{
    public class ListBox : System.Windows.Controls.ListBox
    {
        private Internal.ListBoxUpdater _updater;
        public ListBox()
        {
            //HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            //HorizontalContentAlignment = System.Windows.HorizontalAlignment.Stretch;
            _updater = new Internal.ListBoxUpdater(this);
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            _updater.UpdateListBox(this);
        }

        public void Update() { _updater.UpdateListBox(this); }
    }
}
