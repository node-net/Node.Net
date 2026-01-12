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
            byte[]? bytes = Convert.FromBase64String(base64);
            if (bytes.Length > 0)
            {
                int lastIndex = bytes.Length - 1;
                while (bytes[lastIndex] == 0)
                {
                    lastIndex--;
                }
                if (lastIndex != bytes.Length)
                {
                    List<byte>? new_bytes = new List<byte>();
                    for (int i = 0; i <= lastIndex; ++i)
                    {
                        new_bytes.Add(bytes[i]);
                    }
                    bytes = new_bytes.ToArray();
                }
            }
            MemoryStream? mstream = new MemoryStream(bytes);
            mstream.Seek(0, SeekOrigin.Begin);
            object? result = readFunction(mstream);
            mstream.Close();
            mstream = null;
            return result;
        }

        public static object? Read(this IRead read, Assembly assembly, string name)
        {
            foreach (string? manifestResourceName in assembly.GetManifestResourceNames())
            {
                if (manifestResourceName.Contains(name))
                {
#pragma warning disable CS8604 // Possible null reference argument.
                    return read.Read(assembly.GetManifestResourceStream(manifestResourceName));
#pragma warning restore CS8604 // Possible null reference argument.
                }
            }
            return null;
        }

        public static object? Read(this IRead read, string filename)
        {
            if (filename.Contains("{"))
            {
                MemoryStream? memory = new MemoryStream(Encoding.UTF8.GetBytes(filename));
                return read.Read(memory);
            }
            if (filename.Contains("(") && filename.Contains("*") && filename.Contains(".") && filename.Contains(")") && filename.Contains("|"))
            {
#if IS_WINDOWS
                // open file dialog filter
                Microsoft.Win32.OpenFileDialog? ofd = new Microsoft.Win32.OpenFileDialog { Filter = filename };
                bool? result = ofd.ShowDialog();
                if (result == true)
                {
                    if (File.Exists(ofd.FileName))
                    {
                        FileStream? stream = new FileStream(ofd.FileName, FileMode.Open);
                        //stream.SetFileName(ofd.FileName);
                        object? instance = read.Read(stream);
                        stream.Close();
                        instance?.SetFileName(ofd.FileName);
                        return instance;
                    }
                }
                else
                {
                    return null;
                }
#else
                return null;
#endif
            }
            if (File.Exists(filename))
            {
                using FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
                object? item = read.Read(fs);
                fs.Close();
                SetPropertyValue(item, "FileName", filename);
                item?.SetFileName(filename);

                return item;
            }
            else
            {
                StackTrace? stackTrace = new StackTrace();
                foreach (Assembly? assembly in stackTrace.GetAssemblies())
                {
                    Stream? stream = assembly.GetStream(filename);
                    if (stream != null)
                    {
                        object? item = read.Read(stream);
                        SetPropertyValue(item, "FileName", filename);
                        item?.SetFileName(filename);

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
                PropertyInfo? propertyInfo = item.GetType().GetProperty(propertyName);
                propertyInfo?.SetValue(item, propertyValue);
            }
        }

        public static object? Read(this IRead read, Type type, string name) => Read(read, type.Assembly, name);
    }
}