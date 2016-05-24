namespace Node.Net
{
    public interface IMetaData
    {
        object GetMetaData(object item, string key);

        void SetMetaData(object item, string key, object value);
    }
}
