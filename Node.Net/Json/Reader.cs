using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Node.Net.Json
{
    public class Reader
    {
        public Reader() { }
        public Reader(Assembly assembly) { AddTypes(assembly); }

        public void AddTypes(Assembly assembly)
        {
            foreach (Type type in GetTypes(assembly))
            {
                Types[type.Name] = type;
            }
        }
        public Reader(Assembly[] assemblies)
        {
            foreach (Assembly assembly in assemblies)
            {
                AddTypes(assembly);
            }
        }
        public static Type[] GetTypes(Assembly assembly)
        {
            List<Type> types = new List<System.Type>();
            foreach (Type type in assembly.GetTypes())
            {
                ConstructorInfo ci = type.GetConstructor(Type.EmptyTypes);
                if (!object.ReferenceEquals(null, ci))
                {
                    types.Add(type);
                }
            }
            return types.ToArray();
        }
        public Type DefaultDictionaryType
        {
            get { return reader.DefaultDictionaryType; }
            set { reader.DefaultDictionaryType = value; }
        }

        public Type DefaultListType
        {
            get { return reader.DefaultListType; }
            set { reader.DefaultListType = value; }
        }

        public Dictionary<string,Type> Types
        {
            get { return reader.Types; }
            set { reader.Types = value; }
        }
        private Internal.JsonReader reader = new Internal.JsonReader();
        public object Read(Stream stream) { return reader.Read(stream); }
        public object Read(string value) { return reader.Read(value); }

        public object Read(Stream stream,IDictionary destination)
        {
            IDictionary source = (IDictionary)reader.Read(stream);
            Json.Copier.Copy(source, destination);
            return source;
        }

        public object Read(string value, IDictionary destination)
        {
            IDictionary source = (IDictionary)reader.Read(value);
            Json.Copier.Copy(source, destination);
            return source;
        }

        public object Read(Stream stream, IList destination)
        {
            IList source = (IList)reader.Read(stream);
            Json.Copier.Copy(source, destination);
            return source;
        }

        public object Read(string value, IList destination)
        {
            IList source = (IList)reader.Read(value);
            Json.Copier.Copy(source, destination);
            return source;
        }
    }
}
