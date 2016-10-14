using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Factories.Deprecated.Factories.Helpers
{
    class ArrayHelper
    {
        public static double[] DoubleArrayFromIList(IList list, IFactory factory)
        {
            var doubles = new List<double>();
            foreach (object item in list)
            {
                if (item != null)
                {
                    if (item.GetType().IsPrimitive) doubles.Add(Convert.ToDouble(item));
                }
            }
            return doubles.ToArray();
        }
        public static double[,] DoubleArray2DFromIList(IList list, IFactory factory)
        {
            var dim0 = list.Count;
            var dim1 = 0;
            foreach(var item in list)
            {
                var sublist = item as IList;
                if(sublist != null)
                {
                    if (dim1 < sublist.Count) dim1 = sublist.Count;
                }
            }
            double[,] result = new double[dim0, dim1];
            for(int i = 0; i < list.Count; ++i)
            {
                for(int j = 0; j < dim1; ++j)
                {
                    var value = 0.0;
                    var sublist = list[i] as IList;
                    if(sublist != null)
                    {
                        if (sublist.Count > j) value = Convert.ToDouble(sublist[j]);
                    }
                    result[i, j] = value;
                }
            }
            return result;
        }
    }
}
