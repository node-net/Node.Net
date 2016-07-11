using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Data.Repositories
{
    public static class IRepositoryExtension
    {
        public static object Clone(IRepository repository,object value)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                repository.Writer.Write(memory, value);
                memory.Seek(0, SeekOrigin.Begin);
                return repository.Reader.Read(memory);
            }
        }
    }
}
