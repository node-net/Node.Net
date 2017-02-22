using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Deprecated.Repositories
{
    public class MemoryRepository : Dictionary<string,MemoryStream>, IRepository
    {
        public Func<Stream, object> ReadFunction { get; set; } = Repository.DefaultReadFunction;
        public Action<Stream, object> WriteFunction { get; set; } = Repository.DefaultWriteFunction;

        public string Name { get; set; } = "Memory";
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
        public string[] GetNames(string filter)
        {
            var results = new List<string>();
            foreach(var name in Keys)
            {
                if(filter == null || name.Contains(filter))
                {
                    results.Add(name);
                }
            }
            return results.ToArray();
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
