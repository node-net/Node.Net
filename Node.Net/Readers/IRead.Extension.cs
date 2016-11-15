using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Readers
{
    public static class IReadExtension
    {
        public static object Read(IRead read,Assembly assembly,string name)
        {
            foreach(var manifestResourceName in assembly.GetManifestResourceNames())
            {
                if(manifestResourceName.Contains(name))
                {
                    return read.Read(assembly.GetManifestResourceStream(manifestResourceName));
                }
            }
            return null;
        }
    }
}
