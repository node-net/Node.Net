using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Model
{
    public class Child : IChild
    {
        public IParent Parent { get; set; }
    }
}
