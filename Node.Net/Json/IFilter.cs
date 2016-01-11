namespace Node.Net.Json
{
    public interface IFilter
    {
        bool Include(object value);
    }

}
