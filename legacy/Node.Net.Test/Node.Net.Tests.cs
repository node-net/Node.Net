using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net
{
    class NodeNetTests
    {
        private static Dictionary<string, dynamic> _resources;
        public static IDictionary Resources
        {
            get
            {
                if (ReferenceEquals(null, _resources))
                {
                    _resources = new Dictionary<string, dynamic>();
                    var a = typeof(NodeNetTests).Assembly;
                    foreach (string name in a.GetManifestResourceNames())
                    {
                        _resources.Add(name, Node.Net.Deprecated.Factory.Default.Load(a.GetManifestResourceStream(name), name));
                    }
                }
                return _resources;
            }
        }
    }
}
