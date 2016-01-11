using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace Node.Net.IO
{
    public class StreamExtension
    {
        public static void CopyToFile(Stream source, string fileName)
        {
            if (System.IO.File.Exists(fileName)) System.IO.File.Delete(fileName);
            using (System.IO.FileStream dest = new System.IO.FileStream(fileName, System.IO.FileMode.Create))
            {
                int ibyte = source.ReadByte();
                while (ibyte > -1)
                {
                    dest.WriteByte((byte)ibyte);
                    ibyte = source.ReadByte();
                }
                dest.Flush();
            }
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
            using (StreamReader sr = new StreamReader(source,Encoding.Default,true,1024,true))
            {
                return sr.ReadToEnd();
            }
#endif
        }

#if NET40
        private static StreamWriter streamWriter = null;
#endif
        public static void SetString(Stream stream,string value)
        {
            if(stream.CanWrite)
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

        public static Stream GetStream(string name, Type typeHint)
        {
            Stream result = GetStream(name, typeHint.Assembly);
            if (!object.ReferenceEquals(null, result)) return result;
            return GetStream(name);
        }
        public static Stream GetStream(string name)
        {/*
            Stream result = null;
            if(File.Exists(name))
            {
                return new FileStream(name, FileMode.Open);
            }
            foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies())
            {
                result = GetStream(name, a);
                if (!object.ReferenceEquals(null, result)) return result;
            }

            return result;*/

            //Stream stream = null;
            if (System.IO.File.Exists(name)) return new System.IO.FileStream(name, System.IO.FileMode.Open);
            System.Reflection.Assembly assembly =
                System.Reflection.Assembly.GetCallingAssembly();

            System.IO.Stream stream = GetStream(name, assembly);
            if (!object.ReferenceEquals(null, stream)) return stream;

            foreach (System.Reflection.Assembly a in System.AppDomain.CurrentDomain.GetAssemblies())
            {
                stream = GetStream(name, a);
                if (!object.ReferenceEquals(null, stream)) return stream;
            }
            /*
            System.IO.Stream stream = assembly.GetManifestResourceStream(name);
            if(object.ReferenceEquals(null,stream))
            {
                foreach(string mname in assembly.GetManifestResourceNames())
                {
                    if (mname.Contains(name)) return assembly.GetManifestResourceStream(mname);
                }
                foreach(System.Reflection.Assembly a in System.AppDomain.CurrentDomain.GetAssemblies())
                {
                    stream = a.GetManifestResourceStream(name);
                    if (!object.ReferenceEquals(null, stream)) return stream;
                    foreach (string mname in a.GetManifestResourceNames())
                    {
                        if (mname.Contains(name)) return a.GetManifestResourceStream(mname);
                    }
                }
            }
            return stream;
            */
            throw new System.ArgumentException("ManifestResourceName " + name + " not found.");

        }
        public static Stream GetStream(string name,Assembly assembly)
        {
            string[] rnames = assembly.GetManifestResourceNames();
            foreach(string rname in rnames)
            {
                if (rname.Contains(name)) return assembly.GetManifestResourceStream(rname);
            }


            
            return null;
        }
    }
}
