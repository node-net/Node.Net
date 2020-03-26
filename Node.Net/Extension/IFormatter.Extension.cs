using System.Collections;
using System.IO;
using System.Runtime.Serialization;

namespace Node.Net
{
    public static class IFormatterExtension
    {
        public static object Clone(this IFormatter formatter, object graph)
        {
            using MemoryStream? memory = new MemoryStream();
            formatter.Serialize(memory, graph);
            memory.Seek(0, SeekOrigin.Begin);
            return formatter.Deserialize(memory);
        }

        public static T Clone<T>(this IFormatter formatter, object graph)
        {
            object? item = formatter.Clone(graph);
            if (item is IDictionary dictionary)
            {
                return dictionary.Convert<T>();
            }
            return (T)item;
        }

        public static string GetMD5(this IFormatter formatter, object graph)
        {
            using MemoryStream? memory = new MemoryStream();
            formatter.Serialize(memory, graph);
            memory.Seek(0, SeekOrigin.Begin);
            return memory.GetMD5String();
        }
    }
}