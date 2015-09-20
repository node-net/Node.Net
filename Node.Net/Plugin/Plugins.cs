namespace Node.Net.Plugin
{
    public class Plugins : System.Collections.Generic.Dictionary<string,System.Reflection.Assembly>
    {
        private static Plugins _default = null;
        public static Plugins Default
        {
            get
            {
                if (object.ReferenceEquals(null, _default)) _default = new Plugins();
                return _default;
            }
        }

        public Plugins() { Load(); }

        public string PluginsDirectory => GetAssemblyDirectory(System.Reflection.Assembly.GetAssembly(typeof(Plugins))) + @"\Plugins";
        private void Load()
        {
            // scan Plugins Directory for Plugin Folders
            foreach(string pluginDirectory in System.IO.Directory.GetDirectories(PluginsDirectory))
            {
                System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(pluginDirectory);
                string pluginName = di.Name;
                if(!ContainsKey(pluginName))
                {
                    string pluginPath = pluginDirectory + @"\" + pluginName + ".dll";
                    try
                    {
                        System.Reflection.Assembly pluginAssembly = System.Reflection.Assembly.LoadFrom(pluginPath);
                        Add(pluginName, pluginAssembly);
                    }
                    catch(System.Exception e)
                    {

                    }
                }
            }
        }
        private static string GetAssemblyDirectory(System.Reflection.Assembly assembly)
        {
            string codeBase = assembly.CodeBase;
            System.UriBuilder uri = new System.UriBuilder(codeBase);
            string path = System.Uri.UnescapeDataString(uri.Path);
            return System.IO.Path.GetDirectoryName(path);
        }

        public System.Type GetType(string fullname)
        {
            System.Type result = null;
            foreach(string plugin in Keys)
            {
                result = this[plugin].GetType(fullname);
                if (!object.ReferenceEquals(null, result)) return result;
            }
            return result;
        }

        public object Invoke(string value)
        {
            // test for static method
            if(value.IndexOf('.')>0)
            {
                string[] parts = value.Split('.');
                string methodName = parts[parts.Length-1];
                string typename = value.Replace("." + methodName,"");
                System.Type type = GetType(typename);
                if(!object.ReferenceEquals(null,type))
                {
                    System.Reflection.MethodInfo method = type.GetMethod(methodName);
                    if (!object.ReferenceEquals(null, method)) return method.Invoke(null, null);
                }
            }
            return null;
        }
    }
}
