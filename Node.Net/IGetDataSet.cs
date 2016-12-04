//
// Copyright (c) 2016 Lou Parslow. Subject to the Apache 2.0 license, see LICENSE.txt.
//
using System.Data;

namespace Node.Net
{
    public interface IGetDataSet
    {
        DataSet GetDataSet(string sql);
    }
}
