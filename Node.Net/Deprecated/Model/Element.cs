using System.Collections;
using System.Collections.Generic;

namespace Node.Net.Deprecated.Model
{
    public class Element : Generic.Element<dynamic>
    {

    }
    /*
    public class Element<T> : Parent<T>, IChild, IMetaData
    {
        public IParent Parent { get; set; }

        private readonly Dictionary<string, dynamic> _metaData = new Dictionary<string, dynamic>();
        public IDictionary MetaData { get { return _metaData; } }

    }*/
}
