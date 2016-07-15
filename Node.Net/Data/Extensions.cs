using System.IO;
using System.Reflection;
using System.Security;

namespace Node.Net.Data
{
    public static class Extensions
    {
        public static Stream GetStream(this Assembly assembly, string name) => AssemblyExtension.GetStream(assembly, name);
        public static object Read(this IRead read, string value) => Readers.IReadExtension.Read(read, value);
        public static void Write(this IWrite write, string filename, object value) => Writers.IWriteExtension.Write(write, filename, value);
        public static object Clone(this Repositories.IRepository repository, object value) => Repositories.IRepositoryExtension.Clone(repository, value);
        public static string Protect(this Security.IProtection protection, SecureString secure_secret) => Security.IProtectionExtension.Protect(protection, secure_secret);
        public static SecureString Unprotect(this Security.IProtection protection, string encrypted_secret) => Security.IProtectionExtension.Unprotect(protection, encrypted_secret);
        public static Model.IModel Clone(this Model.IModel model) => Model.IModelExtension.Clone(model);
        public static int ComputeHashCode(this Model.IModel model) => Model.IModelExtension.ComputeHashCode(model);
        public static T Get<T>(this Model.IModel model, string key) => Model.IModelExtension.Get<T>(model, key);
        public static void Set(this Model.IModel model, string key, object value) => Model.IModelExtension.Set(model, key, value);
        public static void SetTypeName(this Model.IModel model) => Model.IModelExtension.SetTypeName(model);
    }
}
