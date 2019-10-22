using System.Collections.Generic;

namespace Node.Net.Writers
{
    public interface IElement
    {
        int Count { get; }
        ICollection<string> Keys { get; }
        dynamic Get(string name);
    }
}
