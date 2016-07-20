using System.IO;
namespace Node.Net.Data
{
    public interface IGet { object Get(string key); }
    public interface IGetKeys { string[] GetKeys(bool deep=false); }
    public interface ISet { void Set(string key, object value); }
    public interface IRead { object Read(Stream stream); }
    public interface IReader { IRead Reader { get; } }
    public interface IWrite { void Write(Stream stream, object value); }
    public interface IWriter { IWrite Writer { get; } }
}
