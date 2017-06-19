using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net
{
    public class Element : Dictionary<string,dynamic>
    {
        public string Name { get { return this.GetName(); } }
    }
}
