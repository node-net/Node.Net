using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Controls.Internal
{
    class DictionaryValue : IDictionaryValue
    {
        public IDictionary IDictionary { get; set; }
        public string Key { get; set; }
    }
}
