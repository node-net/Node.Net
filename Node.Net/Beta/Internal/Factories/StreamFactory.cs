using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
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
            if(name.Contains("(") || name.Contains("*") && name.Contains(".") && name.Contains(")") || name.Contains("|"))
            {
                // open file dialog filter
                var ofd = new Microsoft.Win32.OpenFileDialog { Filter = name };
                var result = ofd.ShowDialog();
                if (result == true)
                {
                    if (File.Exists(ofd.FileName)) return new FileStream(ofd.FileName, FileMode.Open);
                }
            }
            if (name.Contains(":"))
            {
                try
                {
                    var uri = new Uri(name);
                    switch (uri.Scheme)
                    {
                        case "http":
                        case "https":
                            {
                                var webClient = new WebClient();
                                return webClient.OpenRead(name);
                                //break;
                            }
                        case "file":
                            {
                                return new FileStream(uri.LocalPath, FileMode.Open);
                                //break;
                            }
                    }
                }
                catch { }
            }
            return new StackTrace().GetStream(name);
        }
    }
}
