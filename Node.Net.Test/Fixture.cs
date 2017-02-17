using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Tests
{
    public class Fixture
    {
        
        public static object Read(string name)
        {
            var reader = new Node.Net.Reader
            {
                DefaultDocumentType = typeof(Dictionary<string, dynamic>),
                DefaultObjectType = typeof(Dictionary<string, dynamic>)
            };
            return reader.Read(name);
            //return Reader.Default.Read(name);
            //return global::Node.Net.Data.Reader.Default.Read(GetStream(name));
        }
        /*
        public static Stream GetStream(string name)
        {
            var assembly = typeof(Fixture).Assembly;
            foreach (string manifestResourceName in assembly.GetManifestResourceNames())
            {
                if (manifestResourceName.Contains(name))
                {
                    return assembly.GetManifestResourceStream(manifestResourceName);
                }
            }

            return null;
        }*/
    }
}
