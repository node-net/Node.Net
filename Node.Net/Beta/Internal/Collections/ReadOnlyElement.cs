using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Beta.Internal.Collections
{
    class ReadOnlyElement : ReadOnlyDictionary, IReadOnlyElement, IDictionary
    {
        public ReadOnlyElement(IDictionary value) : base(value) { }
        //int Count { get; }
        public bool Contains(string name) { return base.Contains(name); }
        public dynamic Get(string name) { return base[name]; }
        public T Get<T>(string name) { return (T)base[name]; }
        public object Parent { get { return null; } }
        public IDocument Document { get; }
        public string Name { get; }
        public string FullName { get; }
        public string JSON { get; }
        public IEnumerable Find(Type target_type, string pattern = "") { return null; }
    }
}
