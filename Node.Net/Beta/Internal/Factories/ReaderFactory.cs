using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Node.Net.Beta.Internal.Factories
{
    class ReaderFactory : IFactory
    {
        public IFactory ParentFactory { get; set; }

        public object Create(Type target_type, object source)
        {
            if (source != null)
            {
                if (source.GetType() == typeof(string))
                {
                    if (ParentFactory != null)
                    {
                        var stream = ParentFactory.Create<Stream>(source);
                        if (stream != null)
                        {
                            if (ReadFunction != null)
                            {
                                var instance = ReadFunction(stream);
                                var dictionary = instance as IDictionary;
                                if (dictionary != null)
                                {
                                    dictionary = IDictionaryExtension.ConvertTypes(dictionary, IDictionaryTypes, TypeKey);
                                    dictionary.SetFileName(source.ToString());
                                    instance = dictionary;
                                }
                                if (instance != null)
                                {
                                    if (target_type.IsAssignableFrom(instance.GetType())) return instance;
                                    if (ParentFactory != null) return ParentFactory.Create(target_type, instance);
                                }
                            }
                        }
                    }
                }
            }
            return null;
        }
        public Func<Stream, object> ReadFunction { get; set; } = new Internal.Readers.JSONReader().Read;
        public Dictionary<string, Type> IDictionaryTypes { get; set; } = new Dictionary<string, Type>();
        public string TypeKey = "Type";
    }
}
