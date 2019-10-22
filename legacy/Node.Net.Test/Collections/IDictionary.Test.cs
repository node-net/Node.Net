using System.Collections;
using System.Collections.Generic;

namespace Node.Net.Collections
{
    class IDictionaryTest
    {
        public static IDictionary GetLargeDictionary(int depth = 1)
        {
            var dictionary = new Dictionary<string, dynamic>();
            PopulateDictionary(dictionary, depth, 10.0);
            return dictionary;
        }

        private static void PopulateDictionary(IDictionary dictionary, int level, double value)
        {
            switch (level)
            {
                case 0:
                    dictionary["Type"] = "Foo";
                    break;
                case 1:
                    dictionary["Type"] = "Bar";
                    break;
                default:
                    dictionary["Type"] = "Widget";
                    break;
            }
        
            dictionary["Level"] = level;
            dictionary["Length"] = $"{value / ((double)(level))}";
            dictionary["Width"] = "1 m";
            dictionary["Height"] = "1 m";
            dictionary["X"] = "1 m";
            dictionary["Y"] = "1 m";
            dictionary["Z"] = "1 m";

            var child_level = level - 1;
            if (child_level > -1)
            {
                for (int i = 0; i < 13; ++i)
                {
                    var child_key = $"{child_level}-{value}-{i}";
                    var child = new Dictionary<string, dynamic>();
                    PopulateDictionary(child, child_level, value);
                    dictionary[child_key] = child;
                }
            }
        }
    }
}
