using System.IO;
using System.Reflection;

namespace Node.Net.Readers
{
    public static class IReadExtension
    {
        public static object Read(IRead read, Assembly assembly, string name)
        {
            foreach (var manifestResourceName in assembly.GetManifestResourceNames())
            {
                if (manifestResourceName.Contains(name))
                {
                    return read.Read(assembly.GetManifestResourceStream(manifestResourceName));
                }
            }
            return null;
        }

        public static object Read(IRead read,string filename)
        {
            if(File.Exists(filename))
            {
                using (FileStream fs = new FileStream(filename, FileMode.Open))
                {
                    return read.Read(fs);
                }
            }
            return null;
        }
    }
}
