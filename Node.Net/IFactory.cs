using System;
using System.IO;

namespace Node.Net
{
    public interface IFactory : ITransformer
    {
        object Load(Stream stream, string name);
        void Save(object item, Stream stream);

        
    }
}
