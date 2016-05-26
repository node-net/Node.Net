using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Extensions
{
    public class TypeExtension
    {
        public static Stream GetStream(Type type,string name)
        {
            return StreamExtension.GetStream(name, type);
        }
    }
}
