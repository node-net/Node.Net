//
// Copyright (c) 2016 Lou Parslow. Subject to the Apache 2.0 license, see LICENSE.txt.
//
namespace Node.Net
{
    public interface IMetaDataManager
    {
        object GetMetaData(object item, string key);

        void SetMetaData(object item, string key, object value);
    }
}
