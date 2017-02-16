namespace Node.Net
{
    public interface IMetaDataManager
    {
        object GetMetaData(object item, string key);

        void SetMetaData(object item, string key, object value);
    }
}
