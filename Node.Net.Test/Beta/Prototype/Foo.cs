using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Factories.Prototype
{
    class Foo : Dictionary<string,dynamic>, IFoo
    {
        public Foo() { Add("Type", nameof(Foo)); }
    }
}
