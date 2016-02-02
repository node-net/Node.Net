using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;

namespace Node.Net.Controls
{
    class ExampleDataContexts : Dictionary<string,object>
    {
        private static ExampleDataContexts _default = new ExampleDataContexts();
        public static ExampleDataContexts Default
        {
            get { return _default; }
        }
        public ExampleDataContexts()
        {
            initialize();
        }

        public TabControl GetTabControl(Type controlType)
        {
            TabControl tabControl = new TabControl();
            foreach(string key in this.Keys)
            {
                FrameworkElement element = Activator.CreateInstance(controlType) as FrameworkElement;
                if(!object.ReferenceEquals(null, element))
                {
                    element.DataContext = this[key];
                    TabItem tabItem = new TabItem()
                    {
                        Header = key,
                        Content = element
                    };
                    tabControl.Items.Add(tabItem);
                }
            }
            return tabControl;
        }

        private void initialize()
        {
            this["null"] = null;
            this["Positional.Scene"] =
                new KeyValuePair<string,dynamic>(
                       "Positional.Scene",
                       LoadDictionary("Dictionary.Test.Positional.Scene.json"));
        }

        private IDictionary LoadDictionary(string name)
        {
            IDictionary result = null;
            System.IO.Stream stream = IO.StreamExtension.GetStream("Dictionary.Test.Positional.Scene.json");
            if(!object.ReferenceEquals(null, stream))
            {
                Json.Reader reader = new Json.Reader();
                return reader.Read(stream) as IDictionary;
            }
            return result;
        }
    }
}
