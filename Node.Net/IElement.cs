using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net
{
    public interface IElement : Node.Net.Readers.IElement, Node.Net.Writers.IElement, Node.Net.Factories.IElement, Node.Net.Collections.IElement
    {
        void Clear();
        int Count { get; }
        bool Contains(string name);
        dynamic Get(string name);
        T Get<T>(string name);
        void Set(string name, dynamic value);
        ICollection<string> Keys { get; }
        object Parent { get; set; }
        IDocument Document { get; }
        string Name { get; }
        string FullName { get; }
        string JSON { get; }
        IList Find(Type target_type, string pattern = "");
    }
}
