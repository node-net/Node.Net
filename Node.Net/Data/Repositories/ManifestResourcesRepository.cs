using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Node.Net.Data.Repositories
{
    public class ManifestResourcesRepository : IReadOnlyRepository, IReader, IGetKeys
    {
        public IRead Reader => new Readers.Reader();

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
        /*
        private List<Assembly> assemblies = new List<Assembly>();
        public Assembly[] Assemblies
        {
            get
            {
                if (assemblies == null)
                {
                    var list = new List<Assembly>();
                    foreach (AssemblyName assemblyName in Assembly.GetExecutingAssembly().GetReferencedAssemblies())
                    {
                        var asm = Assembly.Load(assemblyName.ToString());
                        if (!list.Contains(asm)) list.Add(asm);
                    }
                    assemblies = list.ToArray();
                }
                return assemblies;
            }
            set { assemblies = value; }
        }
        //private Assembly[] assemblies;
        public Assembly[] Assemblies
        {
            get
            {
                if (assemblies == null)
                {
                    var list = new List<Assembly>();
                    foreach (AssemblyName assemblyName in Assembly.GetExecutingAssembly().GetReferencedAssemblies())
                    {
                        var asm = Assembly.Load(assemblyName.ToString());
                        if (!list.Contains(asm)) list.Add(asm);
                    }
                    assemblies = list.ToArray();
                }
                return assemblies;
            }
            set { assemblies = value; }
        }*/
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
