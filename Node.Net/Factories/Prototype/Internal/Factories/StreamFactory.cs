﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace Node.Net.Factories.Prototype.Internal.Factories
{
    sealed class StreamFactory : IFactory
    {
        public List<Assembly> ResourceAssemblies { get; } = new List<Assembly>();
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
