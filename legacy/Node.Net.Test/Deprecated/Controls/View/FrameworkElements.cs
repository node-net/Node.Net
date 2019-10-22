namespace Node.Net.View
{
    public class FrameworkElements : System.Collections.Generic.Dictionary<string,System.Windows.FrameworkElement>
    {
        public object DataContext
        {
            get
            {
                if (Count > 0) { foreach (string key in Keys) { return this[key].DataContext; } }
                return null;
            }
            set
            {
                foreach (string key in Keys) { this[key].DataContext = value; }
            }
        }

        public static System.Type[] GetAvailableTypes()
        {
            System.Collections.Generic.List<System.Type> types = new System.Collections.Generic.List<System.Type>();
            foreach(System.Reflection.Assembly a in System.AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach(System.Type type in a.GetTypes())
                {
                    if (typeof(System.Windows.FrameworkElement).IsAssignableFrom(type)) types.Add(type);
                }
            }
            return types.ToArray();
        }
    }
}
