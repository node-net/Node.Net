using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace Node.Net.Internal
{
    internal sealed class AbstractFactory : Dictionary<Type, Type>, IFactory
    {
        public IFactory ParentFactory { get; set; }
        public Func<Stream, object> ReadFunction { get; set; } = new Internal.JsonReader().Read;
        public Dictionary<string, Type> IDictionaryTypes { get; set; } = new Dictionary<string, Type>();
        public Type DefaultObjectType { get; set; } = typeof(Dictionary<string, dynamic>);
        public string TypeKey = "Type";

        private bool callingParent = false;
        private CloneFactory CloneFactory { get; } = new CloneFactory();

        public object Create(Type targetType, object source)
        {
            if (targetType == null)
            {
                return null;
            }

            if (source != null && Resources.Contains(source.ToString()))
            {
                object? result = Resources[source.ToString()];
                if (result != null && targetType.IsInstanceOfType(result))
                {
                    return result;
                }
            }
            else
            {
                Stream? stream = CreateStream(source);
                if (stream != null && source != null)
                {
                    string? s = source.ToString();
                    object? result = CreateFromStream(targetType, stream, source);
                    if (result != null)
                    {
                        if (source is string)
                        {
                            Resources.Add(source.ToString(), result);
                        }
                        return result;
                    }
                }
            }

            foreach (Type? _targetType in Keys)
            {
                Type? concreteType = this[_targetType];
                if (_targetType.IsAssignableFrom(targetType))
                {
                    return concreteType.Construct(source);
                }
            }
            object? instance = targetType.Construct(source);
            return instance ?? ResourceFactory.Create(targetType, source) ?? CloneFactory.Create(targetType, source);
        }

        private Stream CreateStream(object source)
        {
            Stream? stream = source as Stream;
            if (stream == null && source != null && ParentFactory != null && !callingParent)
            {
                callingParent = true;
                try
                {
                    stream = ParentFactory.Create(typeof(Stream), source) as Stream;
                }
                finally { callingParent = false; }
            }
            return stream;
        }

        public ResourceDictionary Resources { get; set; } = new ResourceDictionary();
        private readonly ResourceFactory ResourceFactory = new ResourceFactory();

        private object CreateFromStream(Type target_type, Stream stream, object source)
        {
            if (stream == null)
            {
                return null;
            }

            if (ReadFunction != null)
            {
                object? instance = ReadFunction(stream);
                stream.Close();

                if (instance is IDictionary dictionary)
                {
                    IDictionary? new_dictionary = dictionary.ConvertTypes(IDictionaryTypes, DefaultObjectType, TypeKey);
                    new_dictionary.DeepUpdateParents();
                    if (source != null && (source is string))
                    {
                        string filename = stream.GetFileName();
                        if (filename.Length > 0)
                        {
                            new_dictionary.SetFileName(filename);
                        }
                        else
                        {
                            new_dictionary.SetFileName(source.ToString());
                        }
                    }
                    instance = new_dictionary;
                }
                if (instance != null)
                {
                    if (target_type.IsInstanceOfType(instance))
                    {
                        return instance;
                    }

                    if (ParentFactory != null)
                    {
                        return ParentFactory.Create(target_type, instance);
                    }
                }
            }
            return null;
        }
    }
}