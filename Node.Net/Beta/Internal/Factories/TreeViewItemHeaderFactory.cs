﻿using System;
using System.Collections;
using System.Windows.Controls;

namespace Node.Net.Beta.Internal.Factories
{
    class TreeViewItemHeader : Border, ITreeViewItemHeader { }
    class TreeViewItemHeaderFactory
    {
        public object Create(Type targetType, object source)
        {
            if (targetType == null) return null;
            if (!typeof(ITreeViewItemHeader).IsAssignableFrom(targetType)) return null;
            string type = source.GetType().Name;
            string name = source.GetName();

            var idictionary = source as IDictionary;
            if (idictionary != null && name.Length == 0) name = idictionary.GetName();
            return new TreeViewItemHeader
            {
                Child = new Label { Content = $"{type} {name}" }
            };
        }
    }
}
