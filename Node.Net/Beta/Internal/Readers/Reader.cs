using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Node.Net.Beta.Internal.Readers
{
    sealed class Reader
    {
        public Type DefaultDocumentType
        {
            get { return jsonReader.DefaultDocumentType; }
            set { jsonReader.DefaultDocumentType = value; }
        }
        public Type DefaultElementType
        {
            get { return jsonReader.DefaultObjectType; }
            set { jsonReader.DefaultObjectType = value; }
        }
        public Dictionary<string, Type> ConversionTypeNames
        {
            get { return jsonReader.ConversionTypeNames; }
        }

        public dynamic Read(string name)
        {
            var stream = GetStream(name);
            if (stream != null)
            {
                var item = jsonReader.Read(stream);
                item.SetPropertyValue("FileName", name);
                return item;
            }
            throw new Exception($"{name} not found");
        }

        public dynamic Read(Stream stream)
        {
            return jsonReader.Read(stream);
        }

        public static Stream GetStream(string name)
        {
            if (File.Exists(name)) return new FileStream(name, FileMode.Open);
            return new StackTrace().GetStream(name);
        }

        private readonly JSONReader jsonReader = new JSONReader();
    }
}
