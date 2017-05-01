﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Node.Net.Beta.Internal.Factories
{
    class TreeViewItemFactory
    {
        public object Create(Type target_type, object source)
        {
            if (target_type == null) return null;
            if (!typeof(TreeViewItem).IsAssignableFrom(target_type)) return null;
            object header = source.GetType().Name;
            if (ParentFactory != null) header = ParentFactory.Create<ITreeViewItemHeader>(source);
            if (header != null)
            {
                
                var tvi = new TreeViewItem
                {
                    DataContext = source,
                    Header = header,
                    ToolTip = source.GetType().FullName
                };
                UpdateChildren(tvi,source);
                
                return tvi;
            }

            return null;
        }

        private void UpdateChildren(TreeViewItem tvi,object source)
        {
            tvi.Items.Clear();
            IEnumerable children = null;
            if (ParentFactory != null)
            {
                children = ParentFactory.Create<IChildren>(source);
                if (children.GetCount() > 0)
                {
                    foreach(var child in children)
                    {
                        var ctvi = Create(typeof(TreeViewItem), child);
                        if(ctvi != null)
                        {
                            tvi.Items.Add(ctvi);
                        }
                    }
                    //var dummy = new TreeViewItem { Header = "xxxDummyxxx" };
                    //tvi.Items.Add(dummy);
                    //tvi.Expanded += Tvi_Expanded;
                }
            }
        }
        private void Tvi_Expanded(object sender, System.Windows.RoutedEventArgs e)
        {
            var tvi = sender as TreeViewItem;
            foreach(var child in tvi.Items)
            {
                var ctvi = child as TreeViewItem;
            }
            //throw new NotImplementedException();
        }

        public IFactory ParentFactory { get; set; }
    }
}