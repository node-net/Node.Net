using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Collections
{
    public interface IElement
    {
        void Clear();
        int Count { get; }
        bool Contains(string name);
        dynamic Get(string name);
        T Get<T>(string name);
        void Set(string name, dynamic value);
        ICollection<string> Keys { get; }
        IElement Parent { get; }
        IDocument Document { get; }
        string Name { get; }
        string FullName { get; }
        string JSON { get; }
        IEnumerable Find(Type target_type, string pattern = "");
    }
}
