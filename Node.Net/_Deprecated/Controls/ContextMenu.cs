
namespace Node.Net.Deprecated.Controls
{
    public class ContextMenu : System.Windows.Controls.ContextMenu
    {
        public ContextMenu() { DataContextChanged += on_DataContextChanged; }
        public ContextMenu(object dataContext)
        {
            DataContext = dataContext;
            DataContextChanged += on_DataContextChanged;
            Update();
        }
        void on_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            Update();
        }

        public void Update()
        {
            Items.Clear();
            foreach (System.Windows.Controls.MenuItem item in GetMenuItems(Node.Net.Collections.KeyValuePair.GetValue(DataContext)))
            {
                Items.Add(item);
            }
        }

        public static System.Windows.Controls.MenuItem[] GetMenuItems(object value)
        {
            var items =
                new System.Collections.Generic.List<System.Windows.Controls.MenuItem>();
            if (!ReferenceEquals(null, value))
            {
                foreach (System.Reflection.MethodInfo methodInfo in GetMethods(value))
                {
                    var displayName = methodInfo.Name;
                    var attributes = methodInfo.GetCustomAttributes(typeof(System.ComponentModel.DisplayNameAttribute), true);
                    if (attributes.Length == 1)
                    {
                        var dna = attributes[0] as
                            System.ComponentModel.DisplayNameAttribute;
                        displayName = dna.DisplayName;
                    }

                    var menuItem = new System.Windows.Controls.MenuItem
                    {
                        Header = displayName,
                        Command = new MethodInfoCommand(value, methodInfo)
                    };
                    items.Add(menuItem);
                }
            }
            return items.ToArray();
        }
        public static System.Reflection.MethodInfo[] GetMethods(object instance)
        {
            var methods
                = new System.Collections.Generic.List<System.Reflection.MethodInfo>();
            if (!ReferenceEquals(null, instance))
            {
                var methodInfos = instance.GetType().GetMethods(
                    System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
                foreach (System.Reflection.MethodInfo methodInfo in methodInfos)
                {
                    if (methodInfo.ReturnType == typeof(void) &&
                       methodInfo.GetParameters().Length == 0)
                    {
                        var browsable = true;
                        var attributes = methodInfo.GetCustomAttributes(typeof(System.ComponentModel.BrowsableAttribute), true);
                        if (!ReferenceEquals(null, attributes))
                        {
                            foreach (object item in attributes)
                            {
                                var browableAttribute = item as System.ComponentModel.BrowsableAttribute;
                                if (!ReferenceEquals(null, browableAttribute) && !browableAttribute.Browsable) { browsable = false; break; }
                            }
                        }
                        if (browsable) methods.Add(methodInfo);
                    }
                }

            }
            return methods.ToArray();
        }
    }
}
