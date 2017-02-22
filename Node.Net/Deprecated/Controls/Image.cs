using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Deprecated.Controls
{
    public class Image : System.Windows.Controls.Image
    {
        private readonly Internal.ImageUpdater _updater;
        public Image()
        {
            _updater = new Internal.ImageUpdater(this);
        }
    }
}
