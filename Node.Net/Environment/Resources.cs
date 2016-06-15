namespace Node.Net.Environment
{
    public class Resources
    {
        public static System.IO.Stream GetStream(string name)
        {
            return Extensions.StreamExtension.GetStream(name);
        }

        public static System.IO.Stream GetStream(System.Reflection.Assembly assembly,string name)
        {
            return Extensions.StreamExtension.GetStream(name,assembly);
        }

        public static System.IO.Stream GetStream(System.Type type, string name) => GetStream(System.Reflection.Assembly.GetAssembly(type), name);

        public static System.Reflection.Assembly FindAssemblyByManifestResourceName(string name)
        {
            foreach(System.Reflection.Assembly assembly in System.AppDomain.CurrentDomain.GetAssemblies())
            {
                if (!assembly.IsDynamic)
                {
                    foreach (string rname in assembly.GetManifestResourceNames())
                    {
                        if (rname == name) return assembly;
                    }
                }
            }
            return null;
        }

        public static void CopyToFile(System.IO.Stream source, string fileName)
        {
            if (System.IO.File.Exists(fileName)) System.IO.File.Delete(fileName);
            using (System.IO.FileStream dest = new System.IO.FileStream(fileName, System.IO.FileMode.Create))
            {
                var ibyte = source.ReadByte();
                while (ibyte > -1)
                {
                    dest.WriteByte((byte)ibyte);
                    ibyte = source.ReadByte();
                }
                dest.Flush();
            }
        }

        public static void CopyToFile(string resourceName,string fileName)
        {
            if(System.IO.File.Exists(fileName)) System.IO.File.Delete(fileName);
            var fi = new System.IO.FileInfo(fileName);
            if (!System.IO.Directory.Exists(fi.DirectoryName)) System.IO.Directory.CreateDirectory(fi.DirectoryName);
            using(System.IO.FileStream dest = new System.IO.FileStream(fileName,System.IO.FileMode.Create))
            {
                using(System.IO.Stream source = GetStream(resourceName))
                {
                    var ibyte = source.ReadByte();
                    while (ibyte > -1)
                    {
                        dest.WriteByte((byte)ibyte);
                        ibyte = source.ReadByte();
                    }
                }
                dest.Flush();
            }
        }

        public static void CopyToFile(System.Reflection.Assembly assembly,string resourceName,string fileName)
        {
            if (System.IO.File.Exists(fileName)) System.IO.File.Delete(fileName);
            var fi = new System.IO.FileInfo(fileName);
            if (!System.IO.Directory.Exists(fi.DirectoryName)) System.IO.Directory.CreateDirectory(fi.DirectoryName);
            using (System.IO.FileStream dest = new System.IO.FileStream(fileName, System.IO.FileMode.Create))
            {
                using (System.IO.Stream source = GetStream(assembly,resourceName))
                {
                    var ibyte = source.ReadByte();
                    while (ibyte > -1)
                    {
                        dest.WriteByte((byte)ibyte);
                        ibyte = source.ReadByte();
                    }
                }
                dest.Flush();
            }
        }
        public static void CopyToFile(System.Type type,string resourceName,string fileName)
        {
            CopyToFile(System.Reflection.Assembly.GetAssembly(type), resourceName, fileName);
        }
    }
}
