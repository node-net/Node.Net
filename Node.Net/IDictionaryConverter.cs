using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Node.Net
{
    public sealed class IDictionaryConverter
    {
        public Dictionary<string, Type> ConversionTypeNames { get; set; } = new Dictionary<string, Type>();

        public object Convert(IDictionary dictionary)
        {
            var type = dictionary.Get<string>("Type", "");
            if (type.Length > 0 && ConversionTypeNames.ContainsKey(type) && !ConversionTypeNames[type].IsInstanceOfType(dictionary))
            {
                if (!(Activator.CreateInstance(ConversionTypeNames[type]) is IDictionary converted))
                {
                    throw new InvalidOperationException($"Unable to create instance of {ConversionTypeNames[type].FullName}");
                }
                foreach (var key in dictionary.Keys)
                {
                    if (!converted.Contains(key))
                    {
                        converted.Add(key, dictionary[key]);
                    }
                }
                return converted;
            }
            return dictionary;
        }
    }
}
