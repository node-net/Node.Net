using System;
using System.IO;

namespace Node.Net
{
    public class Repository
    {
        public static Repository Default { get; } = new Repository();
        public object Get(string name) => repository.Get(name);
        public void Set(string name, object value) => repository.Set(name, value);
        public Func<Stream,object> ReadFunction
        {
            get { return repository.ReadFunction; }
            set { repository.ReadFunction = value; }
        }
        public Action<Stream,object> WriteFunction
        {
            get { return repository.WriteFunction; }
            set { repository.WriteFunction = value; }
        }
        private global::Node.Net.Repositories.MemoryRepository repository 
            = new Repositories.MemoryRepository
            {
                ReadFunction = Reader.Default.Read,
                WriteFunction =Writer.Default.Write
            };
    }
}
