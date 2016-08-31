using System;
using System.IO;

namespace Node.Net
{
    public interface IFactory { object Create(Type targetType, object source); }
}
