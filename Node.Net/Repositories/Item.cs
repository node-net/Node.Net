namespace Node.Net.Repositories
{
    public class Item
    {
        public IRepository Repository { get; set; }
        public string Key { get; set; }
        public object Value { get { return Repository.Get(Key); } }
    }
}
