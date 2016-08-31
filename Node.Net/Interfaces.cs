using System;
using System.IO;

namespace Node.Net
{
    public interface IRead { object Read(Stream stream); }
    public interface IFactory { object Create(Type targetType, object source); }
}
