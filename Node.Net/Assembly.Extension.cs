//
// Copyright (c) 2016 Lou Parslow. Subject to the MIT license, see LICENSE.txt.
//
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Node.Net
{
    public static class AssemblyExtension
    {
        public static Dictionary<string, Type> GetNameTypeDictionary(this Assembly assembly) => Readers.AssemblyExtension.GetNameTypeDictionary(assembly);
        public static Dictionary<string, Type> GetFullNameTypeDictionary(this Assembly assembly) => Readers.AssemblyExtension.GetFullNameTypeDictionary(assembly);
        public static Stream GetStream(this Assembly assembly, string name) => Extensions.AssemblyExtension.GetStream(assembly, name);
    }
}
