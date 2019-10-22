using System;
using System.Collections;
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

        public static void Import(IRepository destination,IReadOnlyRepository sourceRepository)
        {
            var getKeys = sourceRepository as IGetKeys;
            if(getKeys != null)
            {
                foreach(var key in getKeys.GetKeys())
                {
                    destination.Set(key, sourceRepository.Get(key));
                }
            }
        }
        public static void Import(IRepository destination,IDictionary source)
        {
            foreach(var key in source.Keys)
            {
                destination.Set(key.ToString(), source[key]);
            }
        }
    }
}
