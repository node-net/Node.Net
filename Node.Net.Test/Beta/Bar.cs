using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Beta
{
    interface IBar : IDictionary { }
    class Bar : Dictionary<string,dynamic> , IBar
    {
        public Bar() { Add("Type", nameof(Bar)); }
        public Bar(IDictionary data)
        {
            foreach (var key in data.Keys)
            {
                Add(key.ToString(), data[key]);
            }
        }
    }
}
