using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace Node.Net.Beta.Internal.Factories
{
    sealed class AbstractFactory : Dictionary<Type, Type>, IFactory
    {
        public IFactory ParentFactory { get; set; }
        public Func<Stream, object> ReadFunction { get; set; } = new Internal.Readers.JSONReader().Read;
        public Dictionary<string, Type> IDictionaryTypes { get; set; } = new Dictionary<string, Type>();
        public string TypeKey = "Type";

        private bool callingParent = false;
        private CloneFactory CloneFactory { get; } = new CloneFactory();
        public object Create(Type target_type, object source)
        {
            if (target_type == null) return null;


            //var s = source.ToString();
            if (source != null && Resources.Contains(source.ToString()))
            {
                var result = Resources[source.ToString()];
                if (result != null && target_type.IsAssignableFrom(result.GetType())) return result;
            }
            else
            {
                var stream = CreateStream(source);
                if (stream != null)
                {
                    var s = source.ToString();
                    var result = CreateFromStream(target_type, stream, source);
                    if (result != null)
                    {
                        if (typeof(string).IsAssignableFrom(source.GetType()))
                        {
                            Resources.Add(source.ToString(), result);
                        }
                        return result;
                    }
                }
            }

            foreach (var targetType in Keys)
            {
                var concreteType = this[targetType];
                if (targetType.IsAssignableFrom(target_type))
                {
                    return concreteType.Construct(source);
                }
            }
            var instance =  target_type.Construct(source);
            if (instance != null) return instance;

            instance = ResourceFactory.Create(target_type,source);

            if (instance == null) instance = CloneFactory.Create(target_type, source);
            return instance;
        }

        private Stream CreateStream(object source)
        {
            var stream = source as Stream;
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
        private object CreateFromStream(Type target_type,Stream stream,object source)
        {
            if (stream == null) return null;
            if (ReadFunction != null)
            {
                var instance = ReadFunction(stream);
                stream.Close();
                
                var dictionary = instance as IDictionary;
                if (dictionary != null)
                {
                    var new_dictionary = IDictionaryExtension.ConvertTypes(dictionary, IDictionaryTypes, TypeKey);
                    new_dictionary.DeepUpdateParents();
                    if (source != null && source.GetType() == typeof(string))
                    {
                        string filename = stream.GetFileName();
                        if (filename.Length > 0) new_dictionary.SetFileName(filename);
                        else new_dictionary.SetFileName(source.ToString());
                    }
                    instance = new_dictionary;
                }
                if (instance != null)
                {
                    if (target_type.IsAssignableFrom(instance.GetType())) return instance;
                    if (ParentFactory != null) return ParentFactory.Create(target_type, instance);
                }
            }
            return null;
        }
    }
}
