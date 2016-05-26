using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
        public static string[] GetManifestResourceNames(Type type, string name)
        {
            List<string> results = new List<string>();
            foreach(string resource_name in type.Assembly.GetManifestResourceNames())
            {
                if(resource_name.Contains(name))
                {
                    results.Add(resource_name);
                }
            }
            return results.ToArray();
        }

    }
}
