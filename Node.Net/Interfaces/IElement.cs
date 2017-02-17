using System;
using System.Collections;

namespace Node.Net
{
    public interface IElement : IDictionary
    {
        object Parent { get; set; }
        IDocument Document { get; }
        string Name { get; }
        string FullName { get; }
        string JSON { get; }
        IList Find(Type target_type, string pattern = "");
    }
}
