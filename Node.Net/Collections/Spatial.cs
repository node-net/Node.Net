using System;
using System.Collections.Generic;
using System.IO;

namespace Node.Net.Collections
{
    [Serializable]
    public class Spatial : Element
    {
        public Spatial() : base() { }
        public Spatial(Stream stream) : base(stream) { }
        protected Spatial(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Spatial> Descendants
        {
            get
            {
                return this.Collect<Spatial>();
            }
        }
    }
}
