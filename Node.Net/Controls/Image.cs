using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Controls
{
    public class Image : System.Windows.Controls.Image
    {
        private Internal.ImageUpdater _updater;
        public Image()
        {
            _updater = new Internal.ImageUpdater(this);
        }
    }
}
