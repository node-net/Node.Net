using System.IO;
using System.Reflection;

namespace Node.Net.Data
{
    public static class Extensions
    {
        public static Stream GetStream(this Assembly assembly, string name) => AssemblyExtension.GetStream(assembly, name);
        public static object Read(this IRead read, string value) => Readers.IReadExtension.Read(read, value);
        public static object Clone(this Repositories.IRepository repository, object value) => Repositories.IRepositoryExtension.Clone(repository, value);
    }
}
