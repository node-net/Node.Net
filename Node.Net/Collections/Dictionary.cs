using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Node.Net.Collections
{
    [Serializable]
	public class Dictionary : Dictionary<string, object>, ISerializable
	{
        public Dictionary() { }

		public string ToJson { get { return this.ToJson(); } set { } }

        protected Dictionary(SerializationInfo info, StreamingContext context) : base(info, context) { }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

        public Dictionary Parse(Stream stream)
        {
            return new Reader
            {
                DefaultDocumentType = typeof(Dictionary),
                DefaultObjectType = typeof(Dictionary)
            }.Read(stream) as Dictionary;
        }
    }
}
