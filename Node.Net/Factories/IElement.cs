using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Factories
{
    public interface IElement
    {
        int Count { get; }
        bool Contains(string name);
        ICollection<string> Keys { get; }
        dynamic Get(string name);
        object Parent { get; }
    }
}
