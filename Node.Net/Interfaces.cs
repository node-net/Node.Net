using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net
{
    public interface IRead { object Read(Stream stream); }
    public interface IFactory { object Create(Type targetType, object source); }
}
