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
        public object Create(Type target_type, object source)
        {
            if (target_type == null) return null;

            if (source != null && ParentFactory != null && !callingParent)
            {
                callingParent = true;
                try
                {
                    var stream = ParentFactory.Create<Stream>(source);
                    var item = CreateFromStream(target_type, stream, source);
                    if (item != null) return item;
                }
                finally { callingParent = false; }
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

            //if (ParentFactory != null) return CreateFromStream(target_type, ParentFactory.Create<Stream>(source),source);
            return ResourceFactory.Create(target_type,source);
        }

        public ResourceDictionary Resources { get; set; } = new ResourceDictionary();
        private readonly ResourceFactory ResourceFactory = new ResourceFactory();
        private object CreateFromStream(Type target_type,Stream stream,object source)
        {
            if (stream == null) return null;
            if (ReadFunction != null)
            {
                var instance = ReadFunction(stream);
                var dictionary = instance as IDictionary;
                if (dictionary != null)
                {
                    var new_dictionary = IDictionaryExtension.ConvertTypes(dictionary, IDictionaryTypes, TypeKey);
                    new_dictionary.DeepUpdateParents();
                    if (source != null && source.GetType() == typeof(string))
                    {
                        new_dictionary.SetFileName(source.ToString());
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
