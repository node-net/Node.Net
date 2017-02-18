using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net
{
    public interface IRead
    {
        object Read(Stream stream);
    }
}
