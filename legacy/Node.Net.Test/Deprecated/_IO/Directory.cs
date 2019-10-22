namespace Node.Net.IO
{
    public class Directory
    {
        public static void Create(string directory,System.Collections.IDictionary contents)
        {
            if (directory.Length > 0 && !System.IO.Directory.Exists(directory)) System.IO.Directory.CreateDirectory(directory);
            foreach(object key in contents.Keys)
            {
                string fullName = directory + "\\" + key.ToString();
                System.IO.FileInfo fi = new System.IO.FileInfo(fullName);
                if (!System.IO.Directory.Exists(fi.DirectoryName)) System.IO.Directory.CreateDirectory(fi.DirectoryName);

                byte[] bytes = null;
                if(!object.ReferenceEquals(null,contents[key]) && typeof(byte[]).IsAssignableFrom(contents[key].GetType())) bytes = (byte[])contents[key];

                if (object.ReferenceEquals(null, bytes))
                {
                    using (System.IO.StreamWriter sw = new System.IO.StreamWriter(fullName))
                    {
                        sw.Write(contents[key].ToString());
                    }
                }
                else
                {
                    using (System.IO.FileStream fs = new System.IO.FileStream(fullName, System.IO.FileMode.Create))
                    {
                        foreach (byte b in bytes) { fs.WriteByte(b); }
                    }
                }
            }
        }

        public static string[] Glob(string directory,string pattern,bool deep=false)
        {
            pattern = pattern.Replace('\\', '/');
            System.Collections.Generic.List<string> results = new System.Collections.Generic.List<string>();

            foreach(string filename in System.IO.Directory.EnumerateFiles(directory))
            {
                string relativeFilename = filename.Replace(directory + @"\","" );
                if (IsGlobMatch(relativeFilename, pattern)) results.Add(relativeFilename);
            }
            if(pattern.IndexOf("**/") == 0 || deep)
            {
                foreach(string subdirectory in System.IO.Directory.EnumerateDirectories(directory))
                {
                    string reldir = subdirectory.Replace(directory + @"\", "");
                    foreach(string result in Glob(subdirectory,pattern,true))
                    {
                        results.Add(reldir + @"\" + result);
                    }
                }
            }
            return results.ToArray();
        }

        public static bool IsGlobMatch(string name,string pattern)
        {
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(pattern.Replace("*", ".*?"));
            if(regex.IsMatch(name)) return true;
            if(pattern.IndexOf("**/") == 0)
            {
                regex = new System.Text.RegularExpressions.Regex(pattern.Replace("**/*", "").Replace("*", ".*?"));
                return regex.IsMatch(name);
            }
            return false;
        }

        public static void Zip(string directory,string zipfile)
        {
            System.IO.Compression.ZipFile.CreateFromDirectory(directory, zipfile);
        }

        public static void UnZip(string zipfile,string directory)
        {
            System.IO.Compression.ZipFile.ExtractToDirectory(zipfile, directory);
        }

    }
}
