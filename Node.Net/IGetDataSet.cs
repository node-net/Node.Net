//
// Copyright (c) 2016 Lou Parslow. Subject to the MIT license, see LICENSE.txt.
//
using System.Data;

namespace Node.Net
{
    public interface IGetDataSet
    {
        DataSet GetDataSet(string sql);
    }
}
