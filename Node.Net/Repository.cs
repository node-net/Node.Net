namespace Node.Net
{
    public class Repository
    {
        public static Repository Default { get; } = new Repository();
        public object Get(string name) => repository.Get(name);
        public void Set(string name, object value) => repository.Set(name, value);
        private Node.Net.Repositories.MemoryRepository repository = new Repositories.MemoryRepository();
    }
}
