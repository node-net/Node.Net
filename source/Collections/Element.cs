using System;
using System.IO;

namespace Node.Net.Collections
{
    [Serializable]
    public class Element : Dictionary
    {
        public Element(): base() { }
        public Element(Stream stream) : base(stream) { }
        protected Element(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext)
        {
            throw new NotImplementedException();
        }

        public string Name { get { return this.GetName(); } }

        
    }
}