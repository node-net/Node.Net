using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Model.Generic
{
    public class Element<T> : Parent<T>, IChild
    {
        public IParent Parent { get; set; }

        private readonly Dictionary<string, dynamic> _metaData = new Dictionary<string, dynamic>();

    }
}
