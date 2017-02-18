using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace Node.Net.Readers
{
    public static class IReadExtension
    {
        public static object Read(IRead read, Assembly assembly, string name)
        {
            foreach (var manifestResourceName in assembly.GetManifestResourceNames())
            {
                if (manifestResourceName.Contains(name))
                {
                    return read.Read(assembly.GetManifestResourceStream(manifestResourceName));
                }
            }
            return null;
        }

        public static object Read(IRead read,string filename)
        {
            if(File.Exists(filename))
            {
                using (FileStream fs = new FileStream(filename, FileMode.Open))
                {
                    var item = read.Read(fs);
                    SetPropertyValue(item, "FileName", filename);
                    return item;
                }
            }
            else
            {
                var stackTrace = new StackTrace();
                foreach(var assembly in stackTrace.GetAssemblies())
                {
                    var stream = assembly.GetStream(filename);
                    if (stream != null)
                    {
                        var item = read.Read(stream);
                        SetPropertyValue(item, "FileName", filename);
                        return item;
                    }
                }
                throw new Exception($"{filename} not found");
            }
        }

        private static void SetPropertyValue(object item, string propertyName, object propertyValue)
        {
            if (item != null)
            {
                var propertyInfo = item.GetType().GetProperty(propertyName);
                if (propertyInfo != null)
                {
                    propertyInfo.SetValue(item, propertyValue);
                }
            }
        }
        public static object Read(IRead read, Type type, string name) => Read(read, type.Assembly, name);
    }
}
