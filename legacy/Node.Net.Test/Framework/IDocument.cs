using System;
using System.IO;

namespace Node.Net.Framework
{
    public interface IDocument
    {
        bool ReadOnly { get; }
        void Open(string name,Stream stream);
    }
}
