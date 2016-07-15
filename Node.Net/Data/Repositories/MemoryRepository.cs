using System.Collections.Generic;

namespace Node.Net.Data.Repositories
{
    public class MemoryRepository : Dictionary<string, dynamic>, IRepository
    {
        public IRead Reader { get; set; } = Readers.Reader.Default;
        public IWrite Writer { get; set; } = Writers.Writer.Default;
        public object Get(string key) => ContainsKey(key) ? this[key] : null;
        public void Set(string key, object value)
        {
            this[key] = this.Clone(value);
        }
    }
}
