using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Node.Net
{
    public static class ISerializableExtension
    {
        public static SerializationInfo GetSerializationInfo(this ISerializable value)
        {
            SerializationInfo? serializationInfo = new SerializationInfo(value.GetType(), new FormatterConverter());
            StreamingContext streamingContext = new StreamingContext();
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            MethodInfo mi = value.GetType().GetMethod("GetObjectData", new Type[] { typeof(SerializationInfo), typeof(StreamingContext) });
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            mi.Invoke(value, new object[] { serializationInfo, streamingContext });
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            return serializationInfo;
        }

        public static string ToJson(this ISerializable value)
        {
            using MemoryStream? memory = new MemoryStream();
            new JsonFormatter().Serialize(memory, value);
            memory.Seek(0, SeekOrigin.Begin);
            return new StreamReader(memory).ReadToEnd();
        }

        public static ISerializable Clone(this ISerializable value)
        {
            using MemoryStream? memory = new MemoryStream();
            BinaryFormatter? formatter = new BinaryFormatter();
            formatter.Serialize(memory, value);
            memory.Seek(0, SeekOrigin.Begin);
            return (formatter.Deserialize(memory) as ISerializable)!;
        }
    }
}