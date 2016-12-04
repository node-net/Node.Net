using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Repositories
{
    public class MemoryRepository : Dictionary<string,MemoryStream>
    {
        public Func<Stream, object> ReadFunction { get; set; } = Repository.DefaultReadFunction;
        public Action<Stream, object> WriteFunction { get; set; } = Repository.DefaultWriteFunction;

        public object Get(string name)
        {
            if (ReadFunction == null) return null;
            if(ContainsKey(name))
            {
                this[name].Seek(0, SeekOrigin.Begin);
                return ReadFunction(this[name]);
            }
            return null;
        }
        public void Set(string name, object value)
        {
            if (ContainsKey(name)) Remove(name);
            var memoryStream = new MemoryStream();
            if(WriteFunction != null) WriteFunction(memoryStream, value);
            memoryStream.Flush();
            Add(name, memoryStream);

       
        }
    }
}
