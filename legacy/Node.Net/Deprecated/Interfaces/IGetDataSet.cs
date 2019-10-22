using System.Data;

namespace Node.Net.Deprecated
{
    public interface IGetDataSet
    {
        DataSet GetDataSet(string sql);
    }
}
