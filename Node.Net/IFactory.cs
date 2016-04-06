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

        object Transform(object item, Type type);
    }
}
