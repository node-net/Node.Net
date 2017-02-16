using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Factories.Prototype
{
    class Widget : Dictionary<string,dynamic> ,IWidget
    {
        public Widget() { }
        public Widget(IDictionary data)
        {
            foreach(var key in data.Keys)
            {
                Add(key.ToString(), data[key]);
            }
        }
    }
}
