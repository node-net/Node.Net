using System;
using System.Reflection;
using System.Runtime.Serialization;

namespace Node.Net
{
    public static class ISerializableExtension
    {
        public static SerializationInfo GetSerializationInfo(this ISerializable value)
        {
            var serializationInfo = new SerializationInfo(value.GetType(), new FormatterConverter());
            var streamingContext = new StreamingContext();
            MethodInfo mi = value.GetType().GetMethod("GetObjectData", new Type[] { typeof(SerializationInfo), typeof(StreamingContext) });
            mi.Invoke(value, new object[] { serializationInfo, streamingContext });
            return serializationInfo;
        }
    }
}
