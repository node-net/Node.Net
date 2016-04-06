using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net
{
    public interface IFactory
    {
        object Load(Stream stream, string name);
        void Save(object item, Stream stream);

        object Transform(object item, Type type);
    }
}
