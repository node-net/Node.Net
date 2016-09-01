using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Node.Net.Data
{
    public interface IGet { object Get(string key); }
    public interface IGetKeys { string[] GetKeys(bool deep=false); }
    public interface ISet { void Set(string key, object value); }
    public interface IRead { object Read(Stream stream); }
    public interface IReader { IRead Reader { get; } }
    public interface IWrite { void Write(Stream stream, object value); }
    public interface IWriter { IWrite Writer { get; } }
    public interface IFactory { object Create(Type targetType,object source); }
    public interface IReadOnlyRepository : IGet, IReader { }
    public interface IRepository : IReadOnlyRepository, ISet, IWriter { }
    public interface IChild { IParent Parent { get; set; } }
    public interface IParent { Dictionary<string, IChild> GetChildren(); }
    public interface IDictionaryTypeConverter { IDictionary Convert(IDictionary source); }
}
