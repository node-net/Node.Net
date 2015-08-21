namespace Node.Net.Json
{
    public interface IFilter
    {
        //bool Include(object key,object item);
        bool Include(object value);
    }

}
