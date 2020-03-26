using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;

namespace Node.Net.Internal
{
    internal sealed class StreamFactory : IFactory
    {
        public static List<Assembly> GlobalResourceAssemblies { get; } = new List<Assembly>();
        public List<Assembly> ResourceAssemblies { get; set; } = new List<Assembly>();
        public bool ExactMatch { get; set; } = false;
        private string ignoreFilter;

        public void Refresh()
        {
            ignoreFilter = null;
        }

        public object Create(Type targetType, object source)
        {
            if (targetType == typeof(Stream) && source != null && source is string)
            {
                return Create(source.ToString());
            }
            if (typeof(IStreamSignature).IsAssignableFrom(targetType) && source != null)
            {
                if (source is Stream)
                {
                    return Internal.SignatureReader.GetSignature(source as Stream);
                }
                return Create(targetType, Create(typeof(Stream), source));
            }
            return null;
        }

        public IFactory ParentFactory { get; set; }

        public Stream Create(string name)
        {
            if (File.Exists(name))
            {
                return new FileStream(name, FileMode.Open, FileAccess.Read, FileShare.Read);
            }

            foreach (Assembly? assembly in ResourceAssemblies)
            {
                foreach (string? manifestResourceName in assembly.GetManifestResourceNames())
                {
                    if (manifestResourceName == name)
                    {
                        return assembly.GetManifestResourceStream(manifestResourceName);
                    }
                    if (!ExactMatch && manifestResourceName.Contains(name))
                    {
                        return assembly.GetManifestResourceStream(manifestResourceName);
                    }
                }
            }
            foreach (Assembly? assembly in GlobalResourceAssemblies)
            {
                foreach (string? manifestResourceName in assembly.GetManifestResourceNames())
                {
                    if (manifestResourceName == name)
                    {
                        return assembly.GetManifestResourceStream(manifestResourceName);
                    }
                    if (!ExactMatch && manifestResourceName.Contains(name))
                    {
                        return assembly.GetManifestResourceStream(manifestResourceName);
                    }
                }
            }
            if ((name.Contains("(") || (name.Contains("*") && name.Contains(".") && name.Contains(")")) || name.Contains("|")) && ignoreFilter != name)
            {
                // open file dialog filter
                Microsoft.Win32.OpenFileDialog? ofd = new Microsoft.Win32.OpenFileDialog { Filter = name };
                bool? result = ofd.ShowDialog();
                if (result == true)
                {
                    if (File.Exists(ofd.FileName))
                    {
                        FileStream? stream = new FileStream(ofd.FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                        stream.SetFileName(ofd.FileName);
                        return stream;
                    }
                }
                else
                {
                    // cancelled
                    ignoreFilter = name;
                }
            }
            if (name.Contains(":") && Uri.IsWellFormedUriString(name, UriKind.Absolute))
            {
                Uri? uri = new Uri(name);
                switch (uri.Scheme)
                {
                    case "http":
                    case "https":
                        {
                            using (WebClient? webClient = new WebClient())
                            {
                                return webClient.OpenRead(name);
                            }
                        }
                    case "file":
                        {
                            return new FileStream(uri.LocalPath, FileMode.Open);
                        }
                }
            }
            if (name.Contains("{"))
            {
                MemoryStream? memory = new MemoryStream(Encoding.UTF8.GetBytes(name));
                return memory;
            }
            return new StackTrace().GetStream(name);
        }
    }
}