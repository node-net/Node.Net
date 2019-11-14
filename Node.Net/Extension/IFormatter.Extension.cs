using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;

namespace Node.Net
{
    public static class IFormatterExtension
    {
        public static object Clone(this IFormatter formatter, object graph)
        {
            using var memory = new MemoryStream();
            formatter.Serialize(memory, graph);
            memory.Seek(0, SeekOrigin.Begin);
            return formatter.Deserialize(memory);
        }

        public static T Clone<T>(this IFormatter formatter, object graph)
        {
            var item = formatter.Clone(graph);
            if (item is IDictionary dictionary)
            {
                return dictionary.Convert<T>();
            }
            return (T)item;
        }

        public static string GetMD5(this IFormatter formatter,object graph)
        {
            using var memory = new MemoryStream();
            formatter.Serialize(memory, graph);
            memory.Seek(0, SeekOrigin.Begin);
            return memory.GetMD5String();
        }
    }
}
