using System;
using System.Collections;

namespace Node.Net.Factories.Prototype
{
    public interface IReadOnlyElement
    {
        object Parent { get; }
        string Name { get; }
        string FullName { get; }
        string JSON { get; }
        IEnumerable Find(Type target_type, string pattern = "");
    }
}
