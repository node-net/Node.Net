﻿// Copyright (c) 2016 Lou Parslow. Subject to the MIT license, see LICENSE.txt.
using System.IO;
using System.Reflection;

namespace Node.Net.Extensions
{
    public static class AssemblyExtension
    {
        public static Stream GetStream(this Assembly assembly, string name)
        {
            foreach (var resourceName in assembly.GetManifestResourceNames())
            {
                if (resourceName.Contains(name)) return assembly.GetManifestResourceStream(resourceName);
            }
            return null;
        }
    }
}
