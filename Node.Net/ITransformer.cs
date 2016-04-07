using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net
{
    public interface ITransformer
    {
        IDictionary Resources { get; }
        object Transform(object item, Type type);
    }
}
