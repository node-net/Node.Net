using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net
{
    public class Reader
    {
        private static IReader _default = null;
        public static IReader Default
        {
            get { return _default; }
            set { _default = value; }
        }
    }
}
