namespace Node.Net
{
    public interface IFilter
    {
        bool Include(object value);
    }
}
