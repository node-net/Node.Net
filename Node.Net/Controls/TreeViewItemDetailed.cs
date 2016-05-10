using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;

namespace Node.Net.Controls
{
    public class TreeViewItemDetailed : System.Windows.Controls.TreeViewItem
    {
        private bool showValues = false;
        public bool ShowValues
        {
            get { return showValues; }
            set { showValues = value; Update(); }
        }

        public TreeViewItemDetailed()
        {
            this.DataContextChanged += _DataContextChanged;
        }

        private void _DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            Update();
        }

        protected override void OnExpanded(RoutedEventArgs e)
        {
            Update(true);
            base.OnExpanded(e);
        }
        private bool IsValue(object item)
        {
            if (object.ReferenceEquals(null, item)) return true;
            if (object.ReferenceEquals(null, Collections.KeyValuePair.GetValue(item))) return true;
            if (Collections.KeyValuePair.IsKeyValuePair(item))
            {
                if (Collections.KeyValuePair.GetValue(item).GetType() == typeof(string)) return true;
                IEnumerable ienumerable = Collections.KeyValuePair.GetValue(item) as IEnumerable;
                if (object.ReferenceEquals(null, ienumerable)) return true;
            }
            return false;
        }
        protected virtual void Update(bool expanding = false)
        {
            Header = GetHeader();
            Items.Clear();

            TreeViewItemDetailed[] children = GetChildren();
            if(IsExpanded || expanding)
            {
                foreach (TreeViewItemDetailed tvi in children)
                {
                    Items.Add(tvi);
                }
            }
            else
            {
                Items.Add(new TreeViewItem());
            }
            /*
            if(IsExpanded)
            {

            }
            foreach (TreeViewItemDetailed tvi in GetChildren())
            {
                Items.Add(tvi);
            }*/
        }

        public static bool IsChild(object item)
        {
            if (object.ReferenceEquals(null, item)) return false;
            if (typeof(string).IsAssignableFrom(item.GetType())) return false;
            if (item.GetType().IsPrimitive) return false;
            return true;
        }

        public static object[] GetChildren(object item)
        {
            if (!IsChild(item)) return null;
            IEnumerable ienumerable = item as IEnumerable;
            if (!object.ReferenceEquals(null, ienumerable))
            {
                List<object> children = new List<object>();
                foreach (object child in ienumerable)
                {
                    children.Add(child);
                }
                return children.ToArray();
            }
            return null;
        }

        protected virtual object[] GetChildModels()
        {
            return GetChildren(Collections.KeyValuePair.GetValue(DataContext));
        }
        protected virtual TreeViewItemDetailed[] GetChildren()
        {
            List<TreeViewItemDetailed> children = new List<TreeViewItemDetailed>();
            object[] childmodels = GetChildModels();
            if (!object.ReferenceEquals(null, childmodels))
            {
                foreach (object item in GetChildModels())
                {
                    if (!object.ReferenceEquals(null, item))
                    {
                        TreeViewItemDetailed tvi = Activator.CreateInstance(GetType()) as TreeViewItemDetailed;
                        tvi.IsExpanded = false;
                        tvi.DataContext = item;
                        tvi.ShowValues = ShowValues;
                        children.Add(tvi);
                    }
                }
            }
            return children.ToArray();
        }

        protected virtual object GetHeader()
        {
            XmlElement xmlElement = DataContext as XmlElement;
            if (!object.ReferenceEquals(null, xmlElement))
            {
                return xmlElement.Name;
            }
            if (IsValue(DataContext))
            {
                string key = Collections.KeyValuePair.GetKey(DataContext).ToString();
                string value = Collections.KeyValuePair.GetValue(DataContext).ToString();
                return $"{key} : {value}";
            }
            else
            {
                if (Collections.KeyValuePair.IsKeyValuePair(DataContext))
                {
                    return Collections.KeyValuePair.GetKey(DataContext).ToString();
                }
                return "null";
            }
        }
    }
}
