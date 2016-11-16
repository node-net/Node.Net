using System;
using System.IO;

namespace Node.Net.Repositories
{
    public class DirectoryRepository
    {
        public string Directory { get; set; }
        public Func<Stream, object> ReadFunction { get; set; } = Repository.DefaultReadFunction;
        public Action<Stream, object> WriteFunction { get; set; } = Repository.DefaultWriteFunction;
        public object Get(string name)
        {
            if (!System.IO.Directory.Exists(Directory)) return null;
            if (ReadFunction == null) return null;
            var filename = $"{Directory}\\{name}";
            if (File.Exists(filename))
            {
                using (FileStream fs = new FileStream(filename, FileMode.Open))
                {
                    return ReadFunction(fs);
                }
            }
            return null;
        }
        public void Set(string name, object value)
        {
            if (System.IO.Directory.Exists(Directory))
            {
                if (WriteFunction != null)
                {
                    var filename = $"{Directory}\\{name}";
                    using (FileStream fs = new FileStream(filename, FileMode.Create))
                    {
                        WriteFunction(fs, value);
                    }
                }
            }
        }

    }
}
