﻿using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;

namespace Node.Net.Readers
{
    public static class StreamHelper
    {
        public static Stream GetStream(string name)
        {
            if (Uri.IsWellFormedUriString(name, UriKind.RelativeOrAbsolute))
            {
                if (name.Contains("http:") || name.Contains("https:") || name.Contains("ftp:"))
                {
                    switch (new Uri(name).Scheme)
                    {
                        case "http":
                            {
                                return new WebClient().OpenRead(name);
                            }
                        case "ftp":
                            {
                                return FtpWebRequest.Create(name).GetResponse().GetResponseStream();
                            }
                    }
                }
            }
            if (File.Exists(name)) return new FileStream(name, FileMode.Open);
            else
            {
                var stackTrace = new StackTrace();
                foreach (var assembly in stackTrace.GetAssemblies())
                {
                    var stream = assembly.GetStream(name);
                    if (stream != null) return stream;
                }

            }
            return new MemoryStream(Encoding.UTF8.GetBytes(name));

        }
        public static Stream GetStream(Assembly assembly, string name) => AssemblyExtension.GetStream(assembly, name);
        public static Stream GetStream(Type type, string name) => GetStream(type.Assembly, name);
    }
}
