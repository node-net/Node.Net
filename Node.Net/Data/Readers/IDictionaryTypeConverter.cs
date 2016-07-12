using System.Collections;

namespace Node.Net.Data.Readers
{
    public interface IDictionaryTypeConverter
    {
        IDictionary Convert(IDictionary source);
    }
}
