using System;
using System.Collections;

namespace Node.Net
{
    public interface ITransformer
    {
        IDictionary Resources { get; }
        object Transform(object item, Type type);
    }
}
