using System.Collections.Generic;
using System.IO;

namespace Node.Net.Json
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
            Reader reader = new Reader();
            reader.Read(json,this);
        }


        public override int GetHashCode() => Hash.GetHashCode(this);

        public override bool Equals(object obj)
        {
            if (CompareTo(obj) == 0) return true;
            return false;
        }

        public int CompareTo(object value)
        {
            if (object.ReferenceEquals(this, value)) return 0;
            if (object.ReferenceEquals(null, value)) return 1;

            int thisHash = GetHashCode();
            int thatHash = Hash.GetHashCode(value);
            return GetHashCode().CompareTo(Hash.GetHashCode(value));
        }

        public void Open(Stream stream)
        {
            Reader reader = new Reader();
            reader.Read(stream, this);
        }

        public void Save(Stream stream)
        {
            Writer.Write(this, stream);
        }
    }
}
