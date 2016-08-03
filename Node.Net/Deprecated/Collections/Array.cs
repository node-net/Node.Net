using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Node.Net.Deprecated.Collections
{
    #if NET35
    public class Array : List<object>
    #else
    public class Array : List<dynamic>
    #endif

    {
        public Array() { }
        public Array(string json)
        {
            var reader = new Json.Reader();
            var list = (IList)reader.Read(json);
            foreach (object item in list)
            {
                Add(item);
            }
        }


        public override int GetHashCode() => Deprecated.Collections.Hash.GetHashCode(this);

        public override bool Equals(object obj)
        {
            if (CompareTo(obj) == 0) return true;
            return false;
        }

        public int CompareTo(object value)
        {
            if (object.ReferenceEquals(this, value)) return 0;
            if (object.ReferenceEquals(null, value)) return 1;

            var thisHash = GetHashCode();
            var thatHash = Hash.GetHashCode(value);
            return GetHashCode().CompareTo(Hash.GetHashCode(value));
        }

        public void Open(Stream stream)
        {
            var reader = new Json.Reader();
            Clear();
            var list = (IList)reader.Read(stream);
            foreach (object item in list)
            {
                Add(item);
            }
        }

        public void Save(Stream stream)
        {
            Json.Writer.Write(this, stream);
        }
    }
}
