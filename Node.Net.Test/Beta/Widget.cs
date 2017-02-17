using System.Collections;
using System.Collections.Generic;

namespace Node.Net.Beta
{
    interface IWidget : IDictionary
    {
    }
    class Widget : Dictionary<string, dynamic>, IWidget
    {
        public Widget() { }
        public Widget(IDictionary data)
        {
            foreach (var key in data.Keys)
            {
                Add(key.ToString(), data[key]);
            }
        }
    }
}
