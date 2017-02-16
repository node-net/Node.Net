﻿using System.Collections.Generic;
using System.IO;

namespace Node.Net.Data.Repositories
{
    public class DirectoryRepository : IRepository, IReader, IWriter, IGetKeys
    {
        public string Directory { get; set; } = $"{Path.GetTempPath()}DirectoryRepository";

        public IRead Reader { get; set; } = Node.Net.Data.Reader.Default;
        public IWrite Writer { get; set; } =Writers.Writer.Default;

        public object Get(string key)
        {
            var filename = GetFileName(key);
            if (File.Exists(filename))
            {
                using (FileStream stream = new FileStream(filename, FileMode.Open))
                {
                    return Reader.Read(stream);
                }

            }
            return null;
        }
        public void Set(string key, object value)
        {
            var filename = GetFileName(key);
            var fi = new FileInfo(filename);
            if (!System.IO.Directory.Exists(fi.DirectoryName))
            {
                System.IO.Directory.CreateDirectory(fi.DirectoryName);
            }
            using (FileStream stream = File.OpenWrite(GetFileName(key)))
            {
                Writer.Write(stream, value);
            }
        }

        private string GetFileName(string key) => Path.GetFullPath($"{Directory}\\{key.Replace("/", "\\")}");

        public string[] GetKeys(bool deep)
        {
            var keys = new List<string>();
            SearchOption option = SearchOption.TopDirectoryOnly;
            if (deep) option = SearchOption.AllDirectories;
            foreach(var filename in System.IO.Directory.GetFiles(Directory,"*",option))
            {
                keys.Add(filename.Replace("\\", "/"));
            }
            return keys.ToArray();
        }

    }
}
