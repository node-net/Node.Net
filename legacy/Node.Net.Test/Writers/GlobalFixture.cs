using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Writers.Test
{
    class GlobalFixture
    {
        public static Stream GetStream(string name)
        {
            foreach (var manifestResourceName in typeof(GlobalFixture).Assembly.GetManifestResourceNames())
            {
                if (manifestResourceName == name || manifestResourceName.Contains(name))
                {
                    return typeof(GlobalFixture).Assembly.GetManifestResourceStream(manifestResourceName);
                }
            }
            return null;
        }
    }
}
