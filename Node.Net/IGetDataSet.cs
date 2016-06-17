using System.Data;

namespace Node.Net
{
    public interface IGetDataSet
    {
        DataSet GetDataSet(string sql);
    }
}
