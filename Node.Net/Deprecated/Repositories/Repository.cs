using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Markup;

namespace Node.Net.Deprecated.Repositories
{
    public class Repository
    {
        public static Func<Stream, object> DefaultReadFunction { get; set; } = XamlReader.Load;
        public static Action<Stream, object> DefaultWriteFunction { get; set; } = Write;
        private static void Write(Stream stream, object value) => XamlWriter.Save(value, stream);
        public Func<string, object> AbsoluteGetFunction { get; set; } = new UriRepository().Get;
        //public Func<string, object> AbsoluteSetFunction { get; set; } = new UriRepository().Set;
        public Dictionary<string,Func<string,object>> RelativeGetFunctions { get; set; }
        public object Get(string name)
        {
            if(AbsoluteGetFunction != null)
            {
                var result = AbsoluteGetFunction(name);
                if (result != null) return result;
            }
            if (RelativeGetFunctions != null)
            {
                var parts = name.Split('/');
                if (parts.Length > 1 && RelativeGetFunctions.ContainsKey(parts[0]))
                {
                    var subname = String.Join("/", parts, 1, parts.Length - 1);
                    return RelativeGetFunctions[parts[0]](subname);
                }
            }
            //if (AbsoluteGetFunction != null) return AbsoluteGetFunction(name);
            return null;
        }
        public void Set(string name,object value)
        {

        }
    }
}
