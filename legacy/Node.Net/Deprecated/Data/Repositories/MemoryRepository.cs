using System.Collections.Generic;

namespace Node.Net.Data.Repositories
{
    public class MemoryRepository : Dictionary<string, dynamic>, IRepository, IGetKeys
    {
        public IRead Reader { get; set; } = Node.Net.Data.Reader.Default;
        public IWrite Writer { get; set; } = Writers.Writer.Default;
        public object Get(string key) => ContainsKey(key) ? this[key] : null;
        public void Set(string key, object value)
        {
            this[key] = this.Clone(value);
        }
        public string[] GetKeys(bool deep)
        {
            var keys = new List<string>(Keys);
            return keys.ToArray();
        }
    }
}
