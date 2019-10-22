using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net
{
    interface IBar : IDictionary
    {
        object Parent { get; }
        string Name { get; }
    }
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

        public object Parent { get { return this.GetParent(); } }
        public string Name { get { return this.GetName(); } }
    }
}
