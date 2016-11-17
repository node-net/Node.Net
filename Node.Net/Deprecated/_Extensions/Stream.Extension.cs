using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace Node.Net._Extensions
{
    public class StreamExtension
    {
        public static void CopyToFile(Stream source, string fileName)
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

        public static Stream GetStream(string name, Type typeHint)
        {
            var result = GetStream(name, typeHint.Assembly);
            if (!ReferenceEquals(null, result)) return result;
            return GetStream(name);
        }
        public static Stream GetStream(string name)
        {
            if (System.IO.File.Exists(name)) return new System.IO.FileStream(name, System.IO.FileMode.Open);
            var assembly =
                System.Reflection.Assembly.GetCallingAssembly();

            var stream = GetStream(name, assembly);
            if (!ReferenceEquals(null, stream)) return stream;

            foreach (System.Reflection.Assembly a in System.AppDomain.CurrentDomain.GetAssemblies())
            {
                stream = GetStream(name, a);
                if (!ReferenceEquals(null, stream)) return stream;
            }
            throw new System.ArgumentException("ManifestResourceName " + name + " not found.");

        }
        public static Stream GetStream(string name, Assembly assembly)
        {
            var rnames = assembly.GetManifestResourceNames();
            foreach (string rname in rnames)
            {
                if (rname.Contains(name)) return assembly.GetManifestResourceStream(rname);
            }
            return null;
        }

#if NET40
        private static StreamReader streamReader = null;
#endif
        public static string GetString(Stream source)
        {
            source.Seek(0, SeekOrigin.Begin);
#if NET40
            streamReader = new StreamReader(source);
            return streamReader.ReadToEnd();
#else
            using (StreamReader sr = new StreamReader(source, Encoding.Default, true, 1024, true))
            {
                return sr.ReadToEnd();
            }
#endif
        }

#if NET40
        private static StreamWriter streamWriter = null;
#endif
        public static void SetString(Stream stream, string value)
        {
            if (stream.CanWrite)
            {
#if NET40
                streamWriter = new StreamWriter(stream);
                streamWriter.Write(value);
                streamWriter.Flush();
#else
                using (StreamWriter sw = new StreamWriter(stream, Encoding.Default, 1024, true))
                {
                    sw.Write(value);
                }
#endif
            }

        }
    }
}
