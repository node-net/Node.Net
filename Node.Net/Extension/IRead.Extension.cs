using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;

namespace Node.Net
{
    public static class IReadExtension
    {
        public static object ReadFromBase64String(this IRead read, string base64)
        {
            return ReadFromBase64String(read.Read, base64);
        }

        public static object ReadFromBase64String(Func<Stream, object> readFunction, string base64)
        {
            var bytes = Convert.FromBase64String(base64);
            if (bytes.Length > 0)
            {
                int lastIndex = bytes.Length - 1;
                while (bytes[lastIndex] == 0)
                {
                    lastIndex--;
                }
                if (lastIndex != bytes.Length)
                {
                    var new_bytes = new List<byte>();
                    for (int i = 0; i <= lastIndex; ++i)
                    {
                        new_bytes.Add(bytes[i]);
                    }
                    bytes = new_bytes.ToArray();
                }
            }
            var mstream = new MemoryStream(bytes);
            mstream.Seek(0, SeekOrigin.Begin);
            var result = readFunction(mstream);
            mstream.Close();
            mstream = null;
            return result;
        }
        public static object Read(this IRead read, Assembly assembly, string name)
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

        public static object Read(this IRead read, string filename)
        {
            if (filename.Contains("{"))
            {
                var memory = new MemoryStream(Encoding.UTF8.GetBytes(filename));
                return read.Read(memory);
            }
            if (filename.Contains("(") || filename.Contains("*") && filename.Contains(".") && filename.Contains(")") || filename.Contains("|"))
            {
                // open file dialog filter
                var ofd = new Microsoft.Win32.OpenFileDialog { Filter = filename };
                var result = ofd.ShowDialog();
                if (result == true)
                {
                    if (File.Exists(ofd.FileName))
                    {
                        var stream = new FileStream(ofd.FileName, FileMode.Open);
                        //stream.SetFileName(ofd.FileName);
                        var instance = read.Read(stream);
                        if (instance != null) instance.SetFileName(ofd.FileName);
                        return instance;
                    }
                }
            }
            if (File.Exists(filename))
            {
                using (FileStream fs = new FileStream(filename, FileMode.Open))
                {
                    var item = read.Read(fs);
                    SetPropertyValue(item, "FileName", filename);
                    if (item != null) item.SetFileName(filename);
                    return item;
                }
            }
            else
            {
                var stackTrace = new StackTrace();
                foreach (var assembly in stackTrace.GetAssemblies())
                {
                    var stream = assembly.GetStream(filename);
                    if (stream != null)
                    {
                        var item = read.Read(stream);
                        SetPropertyValue(item, "FileName", filename);
                        if (item != null) item.SetFileName(filename);
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
        public static object Read(this IRead read, Type type, string name) => Read(read, type.Assembly, name);
    }
}
