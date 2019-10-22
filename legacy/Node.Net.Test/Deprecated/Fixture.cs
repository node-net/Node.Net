using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Deprecated
{
    public class Fixture
    {

        public static object Read(string name)
        {
            var reader = new Node.Net.Deprecated.Reader
            {
                DefaultDocumentType = typeof(Dictionary<string, dynamic>),
                DefaultObjectType = typeof(Dictionary<string, dynamic>)
            };
            return reader.Read(name);
        }
    }
}
