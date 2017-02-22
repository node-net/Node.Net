using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net
{
    interface IFoo : IDictionary { object Parent { get; } }
    class Foo : Dictionary<string,dynamic>, IFoo
    {
        public Foo() { Add("Type", nameof(Foo)); }
        public Foo(IDictionary data)
        {
            foreach (var key in data.Keys)
            {
                Add(key.ToString(), data[key]);
            }
        }

        public object Parent { get { return this.GetParent(); } }
    }
}
