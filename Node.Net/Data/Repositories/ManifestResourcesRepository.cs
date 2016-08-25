using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Node.Net.Data.Repositories
{
    public class ManifestResourcesRepository : IReadOnlyRepository, IReader, IGetKeys
    {
        public ManifestResourcesRepository() { }
        public ManifestResourcesRepository(Assembly assembly) { Assemblies.Add(assembly); }
        public IRead Reader { get; set; } = Readers.Reader.Default;

        public object Get(string key)
        {
            var stream = GetResourceStream(key);
            if (stream != null)
            {
                return Reader.Read(stream);
            }
            return null;
        }

        public string[] GetKeys(bool deep)
        {
            var keys = new List<string>();
            foreach (var assembly in Assemblies)
            {
                foreach (var resource_name in assembly.GetManifestResourceNames())
                {
                    keys.Add(resource_name);
                }
            }
            return keys.ToArray();
        }

        public List<Assembly> Assemblies { get; set; } = new List<Assembly>();
        private Stream GetResourceStream(string name)
        {
            foreach (var assembly in Assemblies)
            {


                foreach (var resource_name in assembly.GetManifestResourceNames())
                {
                    if (resource_name.Contains(name)) return assembly.GetManifestResourceStream(resource_name);
                }
            }
            return null;
        }
    }
}
