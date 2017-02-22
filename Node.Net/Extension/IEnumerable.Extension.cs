using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net
{
    public static class IEnumerableExtension
    {
        public static object GetAt(this IEnumerable source, int index) => Beta.Internal.IEnumerableExtension.GetAt(source,index);
    }
}
