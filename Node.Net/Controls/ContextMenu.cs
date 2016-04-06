
namespace Node.Net.Controls
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
            foreach (System.Windows.Controls.MenuItem item in GetMenuItems(Collections.KeyValuePair.GetValue(DataContext)))
            {
                Items.Add(item);
            }
        }

        public static System.Windows.Controls.MenuItem[] GetMenuItems(object value)
        {
            System.Collections.Generic.List<System.Windows.Controls.MenuItem> items =
                new System.Collections.Generic.List<System.Windows.Controls.MenuItem>();
            if (!object.ReferenceEquals(null, value))
            {
                foreach (System.Reflection.MethodInfo methodInfo in GetMethods(value))
                {
                    string displayName = methodInfo.Name;
                    object[] attributes = methodInfo.GetCustomAttributes(typeof(System.ComponentModel.DisplayNameAttribute), true);
                    if (attributes.Length == 1)
                    {
                        System.ComponentModel.DisplayNameAttribute dna = attributes[0] as
                            System.ComponentModel.DisplayNameAttribute;
                        displayName = dna.DisplayName;
                    }

                    System.Windows.Controls.MenuItem menuItem = new System.Windows.Controls.MenuItem();
                    menuItem.Header = displayName;
                    menuItem.Command = new MethodInfoCommand(value, methodInfo);
                    items.Add(menuItem);
                }
            }
            return items.ToArray();
        }
        public static System.Reflection.MethodInfo[] GetMethods(object instance)
        {
            System.Collections.Generic.List<System.Reflection.MethodInfo> methods
                = new System.Collections.Generic.List<System.Reflection.MethodInfo>();
            if (!object.ReferenceEquals(null, instance))
            {
                System.Reflection.MethodInfo[] methodInfos = instance.GetType().GetMethods(
                    System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
                foreach (System.Reflection.MethodInfo methodInfo in methodInfos)
                {
                    if (methodInfo.ReturnType == typeof(void) &&
                       methodInfo.GetParameters().Length == 0)
                    {
                        bool browsable = true;
                        object[] attributes = methodInfo.GetCustomAttributes(typeof(System.ComponentModel.BrowsableAttribute), true);
                        if (!object.ReferenceEquals(null, attributes))
                        {
                            foreach (object item in attributes)
                            {
                                System.ComponentModel.BrowsableAttribute browableAttribute = item as System.ComponentModel.BrowsableAttribute;
                                if (!object.ReferenceEquals(null, browableAttribute) && browableAttribute.Browsable == false) { browsable = false; break; }
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
