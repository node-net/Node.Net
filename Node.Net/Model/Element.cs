using System.Collections;
using System.Collections.Generic;

namespace Node.Net.Model
{
    public class Element : Parent, IChild, IMetaData
    {
        public IParent Parent { get; set; }

        private readonly Dictionary<string, dynamic> _metaData = new Dictionary<string, dynamic>();
        public IDictionary MetaData { get { return _metaData; } }

    }
}
