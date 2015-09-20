namespace Node.Net.Environment
{
    public class Environment
    {
        public static string DevRoot
        {
            get
            {
                string dev_root = System.Environment.GetEnvironmentVariable("DEV_ROOT");
                {
                    if(!object.ReferenceEquals(null,dev_root))
                    {
                        return dev_root.Replace('/','\\');
                    }
                }
                return System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
            }
        }

        public static string GetShortName(System.Reflection.Assembly assembly)
        {
            string[] parts = assembly.FullName.Split(',');
            return parts[0];
        }
        public static string GetWorkingDirectory(System.Type type) => GetWorkingDirectory(System.Reflection.Assembly.GetAssembly(type));
        public static string GetWorkingDirectory(System.Reflection.Assembly assembly)
        {
            string working_dir = ComputeWorkingDirectory(assembly);

            System.Windows.ResourceDictionary cache = GetWorkingDirectories();
            if(cache.Contains(GetShortName(assembly)))
            {
                string cache_name = cache[GetShortName(assembly)].ToString();
                if (working_dir.Length == 0) working_dir = cache_name;
                else
                {
                    if(cache_name != working_dir)
                    {
                        cache[GetShortName(assembly)] = working_dir;
                        SaveWorkingDirectories();
                    }
                }
            }
            else
            {
                cache[GetShortName(assembly)] = working_dir;
                SaveWorkingDirectories();
            }
            return working_dir;
        }

        private static System.Windows.ResourceDictionary workingDirectories = null;
        private static System.Windows.ResourceDictionary GetWorkingDirectories()
        {
            if(object.ReferenceEquals(null,workingDirectories))
            {
                if (System.IO.File.Exists(WorkingDirectoriesFilename))
                {
                    try
                    {
                        using (System.IO.FileStream fs = new System.IO.FileStream(WorkingDirectoriesFilename, System.IO.FileMode.Open))
                        {
                            workingDirectories = System.Windows.Markup.XamlReader.Load(fs) as System.Windows.ResourceDictionary;
                        }
                    }
                    catch { workingDirectories = new System.Windows.ResourceDictionary(); }
                }
                else { workingDirectories = new System.Windows.ResourceDictionary(); }
            }

            return workingDirectories;
        }
        private static string WorkingDirectoriesFilename => System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData)
    + @"\WorkingDirectories.xaml";
        private static void SaveWorkingDirectories()
        {
            using(System.IO.FileStream fs = new System.IO.FileStream(WorkingDirectoriesFilename,System.IO.FileMode.Create))
            {
                System.Windows.Markup.XamlWriter.Save(GetWorkingDirectories(), fs);
            }
        }
        private static string ComputeWorkingDirectory(System.Reflection.Assembly assembly)
        {
            string working_dir = "";
            string dll_filename = assembly.CodeBase.Replace("file:///", "").Replace('/', '\\');

            System.IO.FileInfo fi = new System.IO.FileInfo(dll_filename);

            bool foundName = false;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            string[] words = dll_filename.Split('\\');
            foreach(string word in words)
            {
                if (sb.Length > 0) sb.Append("\\");
                sb.Append(word);
                if(foundName && word == "trunk")
                {
                    sb.Append("trunk");
                    working_dir = sb.ToString();
                }
                if(word == fi.Name || word == fi.Name.Replace(fi.Extension,"").Replace(".Test",""))
                {
                    working_dir = sb.ToString();
                    foundName = true;
                }
            }
            if(foundName)
            {

                return fi.DirectoryName;
            }

            return "";// working_dir;
        }
    }
}
