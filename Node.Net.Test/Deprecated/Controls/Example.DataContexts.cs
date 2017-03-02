﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;

namespace Node.Net.Deprecated.Controls
{
    class ExampleDataContexts : Dictionary<string,object>
    {
        private static readonly ExampleDataContexts _default = new ExampleDataContexts();
        public static ExampleDataContexts Default
        {
            get { return _default; }
        }
        public ExampleDataContexts()
        {
            initialize();
        }

        public Deprecated.Controls.TabControl GetTabControl(Type controlType)
        {
            var tabControl = new Deprecated.Controls.TabControl();
            foreach (string key in this.Keys)
            {
                var element = Activator.CreateInstance(controlType) as FrameworkElement;
                if (!ReferenceEquals(null, element))
                {
                    element.DataContext = this[key];
                    var tabItem = new Deprecated.Controls.TabItem
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

        private static IDictionary LoadDictionary(string name)
        {
            const IDictionary result = null;
            var stream = Extensions.StreamExtension.GetStream("Dictionary.Test.Positional.Scene.json");
            if (!ReferenceEquals(null, stream))
            {
                var reader = new Json.Reader();
                return reader.Read(stream) as IDictionary;
            }
            return result;
        }
    }
}