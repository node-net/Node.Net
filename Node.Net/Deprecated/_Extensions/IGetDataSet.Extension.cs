using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Extensions
{
    public static class IGetDataSetExtension
    {
        public static string[] GetStringArray(IGetDataSet getDataSet,string sql)
        {
            var results = new List<string>();
            foreach (DataRow row in getDataSet.GetDataSet(sql).Tables[0].Rows)
            {
                results.Add(row[0].ToString());
            }
            return results.ToArray();
        }
    }
}
