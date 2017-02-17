using System.Collections;
using System.Collections.Generic;

namespace Node.Net.Beta.Internal.Collections
{
    sealed class Dictionary : ReadOnlyDictionary
    {
        public Dictionary() : base(new Dictionary<string, dynamic>()) { }
        public Dictionary(IDictionary data) : base(data) { }
        public override bool IsReadOnly { get { return false; } }
        public override bool IsFixedSize { get { return false; } }
        public override void Add(object key, object value) { data.Add(key, value); }
        public override void Clear() { data.Clear(); }
        public override void Remove(object key) { data.Remove(key); }
    }
}
