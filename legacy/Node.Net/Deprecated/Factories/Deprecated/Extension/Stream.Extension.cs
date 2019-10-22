using System;
using System.Collections.Generic;
using System.IO;

namespace Node.Net.Factories.Deprecated.Extension
{
    static class StreamExtension
    {
        private static readonly Dictionary<WeakReference<Stream>, string> StreamNames = new Dictionary<WeakReference<Stream>, string>();
        public static string GetName(Stream stream)
        {
            foreach (var reference in StreamNames.Keys)
            {
                Stream target = null; ;
                if (reference.TryGetTarget(out target))
                {
                    if (target == stream) return StreamNames[reference];
                }
            }
            return string.Empty;
        }
        public static void SetName(Stream stream, string name)
        {
            foreach (var reference in StreamNames.Keys)
            {
                Stream target = null;
                if (reference.TryGetTarget(out target))
                {
                    if (target == stream)
                    {
                        StreamNames[reference] = name;
                        return;
                    }
                }
            }
            StreamNames.Add(new WeakReference<Stream>(stream), name);

        }
        public static void FlushNames(Stream stream)
        { StreamNames.Clear(); }
    }
}
