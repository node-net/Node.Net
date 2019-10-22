using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;

namespace Node.Net.Collections
{
    public static class Resources
    {
        public static object Get(string key)
        {
            if (ResourceDictionary.Contains(key)) return ResourceDictionary[key];
            return null;
        }
        private static ResourceDictionary resourceDictionary;
        public static ResourceDictionary ResourceDictionary
        {
            get
            {
                if(resourceDictionary == null)
                {
                    resourceDictionary = XamlReader.Load(typeof(Resources).Assembly.GetManifestResourceStream("Node.Net.Collections.ResourceDictionary.xaml")) as ResourceDictionary;
                }
                return resourceDictionary;
            }
        }
    }
}
