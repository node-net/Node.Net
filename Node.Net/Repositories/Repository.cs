using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Markup;

namespace Node.Net.Repositories
{
    public class Repository
    {
        public static Func<Stream, object> DefaultReadFunction { get; set; } = XamlReader.Load;
        public static Action<Stream, object> DefaultWriteFunction { get; set; } = Write;
        private static void Write(Stream stream, object value) => XamlWriter.Save(value, stream);
        public Func<string, object> AbsoluteGetFunction { get; set; } = new UriRepository().Get;
        //public Func<string, object> AbsoluteSetFunction { get; set; } = new UriRepository().Set;
        public Dictionary<string,Func<string,object>> RelativeGetFunctions { get; set; }
    }
}
