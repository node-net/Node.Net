namespace Node.Net
{
    public interface IRepository
    {
        IReader Reader { get; set; }
        IWriter Writer { get; set; }
        bool IsReadOnly { get; }    // TODO: leverage Writer to eliminate this property
        void Add(string key, object value);
        bool Contains(string key);
        object Get(string key);
        bool Remove(string key);
        string[] Find(string pattern);
    }
}
