using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Node.Net.Collections
{
    [Serializable]
	public class Dictionary : Dictionary<string, object>, ISerializable
	{
        public Dictionary() { }

		public string Json { get { return this.ToJson(); } set { } }

        protected Dictionary(SerializationInfo info, StreamingContext context) : base(info, context) { }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}
