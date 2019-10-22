using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace Node.Net.Factories
{
    class GlobalFixture
    {
        public static object Read(string name)
        {
            var assembly = typeof(GlobalFixture).Assembly;
            foreach(var manifestResourceName in assembly.GetManifestResourceNames())
            {
                if(manifestResourceName.Contains(name))
                {
                    return Read(assembly.GetManifestResourceStream(manifestResourceName), manifestResourceName);
                }
            }
            return null;
        }

        public static object Read(Stream stream,string name)
        {
            if(name.Contains(".json"))
            {
                var reader = new Node.Net.Readers.JsonReader();
                return reader.Read(stream);
            }
            return XamlReader.Load(stream);
        }

        public static Stream GetStream(string name)
        {
            var assembly = typeof(GlobalFixture).Assembly;
            foreach (var manifestResourceName in assembly.GetManifestResourceNames())
            {
                if (manifestResourceName.Contains(name))
                {
                    return assembly.GetManifestResourceStream(manifestResourceName);
                }
            }
            return null;
        }

        public static IDictionary GetDictionaryModel(string name)
        {
            var reader = new Node.Net.Readers.JsonReader();
            var models = new Dictionary<string, dynamic>();
            foreach (var resourceName in typeof(GlobalFixture).Assembly.GetManifestResourceNames())
            {
                if (resourceName.Contains(name))
                {
                    return reader.Read(GetStream(resourceName)) as IDictionary;
                }
            }
            return new Dictionary<string, dynamic>();
        }
        public static IDictionary GetDictionaryModels()
        {
            var reader = new Node.Net.Readers.JsonReader();
            var models = new Dictionary<string, dynamic>();
            foreach (var resourceName in typeof(GlobalFixture).Assembly.GetManifestResourceNames())
            {
                try
                {
                    if (resourceName.Contains(".json"))
                    {
                        models.Add(resourceName, reader.Read(GetStream(resourceName)));
                    }
                }
                catch (System.Exception e)
                {
                    throw new System.Exception($"reading resource {resourceName}", e);
                }
            }
            return models;
        }
    }
}
