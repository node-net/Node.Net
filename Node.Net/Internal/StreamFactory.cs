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
        private string? ignoreFilter;

        public void Refresh()
        {
            ignoreFilter = null;
        }

        public object Create(Type targetType, object source)
        {
            if (targetType == typeof(Stream) && source != null && source is string)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                return Create(source.ToString());
#pragma warning restore CS8604 // Possible null reference argument.
            }
            if (typeof(IStreamSignature).IsAssignableFrom(targetType) && source != null)
            {
                if (source is Stream)
                {
#pragma warning disable CS8604 // Possible null reference argument.
                    return Internal.SignatureReader.GetSignature(source as Stream);
#pragma warning restore CS8604 // Possible null reference argument.
                }
                return Create(targetType, Create(typeof(Stream), source));
            }
#pragma warning disable CS8603 // Possible null reference return.
            return null;
#pragma warning restore CS8603 // Possible null reference return.
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
#pragma warning disable CS8603 // Possible null reference return.
                        return assembly.GetManifestResourceStream(manifestResourceName);
#pragma warning restore CS8603 // Possible null reference return.
                    }
                    if (!ExactMatch && manifestResourceName.Contains(name))
                    {
#pragma warning disable CS8603 // Possible null reference return.
                        return assembly.GetManifestResourceStream(manifestResourceName);
#pragma warning restore CS8603 // Possible null reference return.
                    }
                }
            }
            foreach (Assembly? assembly in GlobalResourceAssemblies)
            {
                foreach (string? manifestResourceName in assembly.GetManifestResourceNames())
                {
                    if (manifestResourceName == name)
                    {
#pragma warning disable CS8603 // Possible null reference return.
                        return assembly.GetManifestResourceStream(manifestResourceName);
#pragma warning restore CS8603 // Possible null reference return.
                    }
                    if (!ExactMatch && manifestResourceName.Contains(name))
                    {
#pragma warning disable CS8603 // Possible null reference return.
                        return assembly.GetManifestResourceStream(manifestResourceName);
#pragma warning restore CS8603 // Possible null reference return.
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
							using WebClient? webClient = new WebClient();
							return webClient.OpenRead(name);
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