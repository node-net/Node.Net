using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace Node.Net.Data.Repositories
{
    public class ZipRepository : IRepository, IReader, IWriter, IGetKeys
    {
        public string FileName { get; set; } = $"{Path.GetTempPath()}ZipRepository.zip";

        public IRead Reader { get; set; } = Readers.Reader.Default;
        public IWrite Writer { get; set; } = Writers.Writer.Default;

        public object Get(string key)
        {
            if (File.Exists(FileName))
            {
                using (ZipArchive archive = ZipFile.OpenRead(FileName))
                {
                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        if (entry.FullName == key)
                        {
                            return Reader.Read(entry.Open());
                        }
                    }
                }
            }
            return null;
        }

        public void Set(string key, object value)
        {
            using (FileStream fs = new FileStream(FileName, FileMode.OpenOrCreate))
            {
                using (ZipArchive archive = new ZipArchive(fs, ZipArchiveMode.Create))
                {
                    var entry = archive.CreateEntry(key);
                    Writer.Write(entry.Open(), value);
                }
            }
        }

        public string[] GetKeys(bool deep)
        {
            var keys = new List<string>();
            if (File.Exists(FileName))
            {
                using (ZipArchive archive = ZipFile.OpenRead(FileName))
                {
                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        keys.Add(entry.FullName.Replace("\\", "/"));
                    }
                }
            }
            return keys.ToArray();
        }
    }
}
