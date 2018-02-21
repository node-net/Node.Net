﻿using System;
using System.Collections;
using System.IO;

namespace Node.Net.Beta.Internal.Factories
{
    sealed class CollectionsFactory : IFactory
    {
        public object Create(Type targetType, object source)
        {
            if (source != null)
            {
                if (source.GetType() == typeof(string))
                {
                    var stream = StreamFactory.Create(source.ToString());
                    if (stream != null)
                    {
                        var instance = Create(targetType, stream);
                        var idictionary = instance as IDictionary;
                        if (idictionary != null) idictionary.SetFileName(source.ToString());
                        return instance;
                    }
                }
            }
            if (targetType == typeof(IDictionary))
            {
                var stream = source as Stream;
                if (stream != null) return JSONReader.Read(stream) as IDictionary;
            }
            if (targetType == typeof(IList))
            {
                var stream = source as Stream;
                if (stream != null) return JSONReader.Read(stream) as IList;
            }
            return null;
        }

        public StreamFactory StreamFactory { get; set; } = new StreamFactory();
        private readonly Internal.Readers.JSONReader JSONReader = new Internal.Readers.JSONReader();
    }
}
