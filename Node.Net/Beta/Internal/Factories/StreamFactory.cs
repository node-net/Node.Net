using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace Node.Net.Beta.Internal.Factories
{
    sealed class StreamFactory : IFactory
    {
        public List<Assembly> ResourceAssemblies { get; set; } = new List<Assembly>();
        public bool ExactMatch { get; set; } = false;
        public object Create(Type target_type, object source)
        {
            if (target_type == typeof(Stream))
            {
                if (source != null)
                {
                    if (source.GetType() == typeof(string)) return Create(source.ToString());
                }
            }
            if(typeof(IStreamSignature).IsAssignableFrom(target_type))
            {
                if(source != null)
                {
                    if(typeof(Stream).IsAssignableFrom(source.GetType()))
                    {
                        return Internal.Readers.SignatureReader.GetSignature(source as Stream);
                        /*
                        var sr = new Internal.Readers.SignatureReader();
                        return sr.Read(source as Stream) as Internal.Readers.Signature;*/
                    }
                    return Create(target_type, Create(typeof(Stream), source));
                }
            }
            return null;
        }

        public Stream Create(string name)
        {
            if (File.Exists(name)) return new FileStream(name, FileMode.Open);
            foreach (var assembly in ResourceAssemblies)
            {
                foreach (var manifestResourceName in assembly.GetManifestResourceNames())
                {
                    if (manifestResourceName == name) return assembly.GetManifestResourceStream(manifestResourceName);
                    if (!ExactMatch && manifestResourceName.Contains(name)) return assembly.GetManifestResourceStream(manifestResourceName);
                }
            }
            return new StackTrace().GetStream(name);
        }
    }
}
