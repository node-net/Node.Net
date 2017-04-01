using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net
{
    public class Writer
    {
        public static Writer Default { get; } = new Writer();

        public Writer()
        {
            WriteFunctions = new Dictionary<Type, Action<Stream, object>>
            {
                {typeof(IDictionary),jsonWriter.Write },
                {typeof(IEnumerable),jsonWriter.Write }
            };
        }
        public void Write(Stream stream, object value)
        {
            if (value != null && WriteFunctions != null)
            {
                foreach (var type in WriteFunctions.Keys)
                {
                    if (type.IsAssignableFrom(value.GetType()))
                    {
                        WriteFunctions[type](stream, value);
                        return;
                    }
                }
            }
            jsonWriter.Write(stream, value);
        }
        private Dictionary<Type, Action<Stream, object>> WriteFunctions { get; set; }
        private JSONWriter jsonWriter = new JSONWriter();
    }
}
