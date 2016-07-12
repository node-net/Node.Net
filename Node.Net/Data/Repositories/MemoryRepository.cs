using System.Collections.Generic;

namespace Node.Net.Data.Repositories
{
    public class MemoryRepository : Dictionary<string, dynamic>, IRepository
    {
        public IRead Reader => new Readers.Reader();
        public IWrite Writer => new Writers.PrimitiveWriter();
        public object Get(string key) => ContainsKey(key) ? this[key] : null;
        public void Set(string key, object value)
        {
            this[key] = this.Clone(value);
        }
    }
}
