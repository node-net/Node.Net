using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Deprecated.Collections
{
    public class Item
    {
        public string Key { get; set; }
        public object Value { get; set; }
        public string ValueString { get { return Value.ToString(); } }
    }
}
