using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Node.Net
{
    public static class SerializationInfoExtension
    {
        public static IDictionary GetPersistentData(this SerializationInfo info)
        {
            Dictionary<string, object>? data = new Dictionary<string, object>();
            SerializationInfoEnumerator e = info.GetEnumerator();
            while (e.MoveNext())
            {
#pragma warning disable CS8604 // Possible null reference argument.
                data.Add(e.Name, e.Value);
#pragma warning restore CS8604 // Possible null reference argument.
            }
            return data;
        }

        public static void SetPersistentData(this SerializationInfo info, IDictionary data)
        {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            foreach (string key in data.Keys)
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            {
#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CS8604 // Possible null reference argument.
                info.AddValue(key, data[key]);
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8604 // Possible null reference argument.
            }
        }
    }
}