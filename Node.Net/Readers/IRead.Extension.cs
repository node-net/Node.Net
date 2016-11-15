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
    }
}
