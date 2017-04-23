using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Node.Net.Beta.Internal.Factories
{
    class TreeViewItemHeader : Border, ITreeViewItemHeader { }
    class TreeViewItemHeaderFactory
    {
        public object Create(Type target_type, object source)
        {
            if (target_type == null) return null;
            if (!typeof(ITreeViewItemHeader).IsAssignableFrom(target_type)) return null;
            string type = source.GetType().Name;
            string name = source.GetName();

            var idictionary = source as IDictionary;
            if (idictionary != null) name = idictionary.GetName();
            //return $"{type} {name}";
            return new TreeViewItemHeader
            {
                Child = new Label { Content = $"{type} {name}" }
            };
        }
    }
}
