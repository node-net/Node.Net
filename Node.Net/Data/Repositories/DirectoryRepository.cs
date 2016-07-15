using System.IO;

namespace Node.Net.Data.Repositories
{
    public class DirectoryRepository : IRepository, IReader, IWriter
    {
        public string Directory { get; set; } = $"{Path.GetTempPath()}DirectoryRepository";

        public IRead Reader { get; set; } = Readers.Reader.Default;
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
    }
}
