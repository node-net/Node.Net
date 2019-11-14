using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;

namespace Node.Net.Collections
{
    [Serializable]
	public class Dictionary : Dictionary<string, object>, ISerializable, IComparable
	{
        #region Construction
        public Dictionary() { }
        #endregion
        public string ToJson { get { return this.ToJson(); } set { } }

        #region Serialization
        protected Dictionary(SerializationInfo info, StreamingContext context) : base(info, context) { }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
        #endregion

        public override bool Equals(object? obj)
        {
            if (obj is null) return false;
            return GetHashCode().Equals(obj.GetHashCode());
        }

        public override int GetHashCode()
        {
            return this.ComputeHashCode();
        }

        public int CompareTo(object? obj)
        {
            if (obj is null) return 1;
            if (object.ReferenceEquals(this, obj)) return 0;
            return GetHashCode().CompareTo(obj.GetHashCode());
        }

        public static Dictionary Parse(Stream stream)
        {
            return new Reader
            {
                DefaultDocumentType = typeof(Dictionary),
                DefaultObjectType = typeof(Dictionary)
            }.Read(stream) as Dictionary;
        }

        public static Dictionary Parse(string json)
        {
            return Parse(new MemoryStream(Encoding.UTF8.GetBytes(json)));
        }
    }
}
