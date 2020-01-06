using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace Node.Net
{
    public static class StackTraceExtension
    {
        public static Assembly[] GetAssemblies(this StackTrace stackTrace)
        {
            var assemblies = new List<Assembly>();
            for (int f = 1; f < stackTrace.FrameCount; ++f)
            {
                var assembly = stackTrace.GetFrame(f).GetMethod().DeclaringType.Assembly;
                if (!assemblies.Contains(assembly))
                {
                    assemblies.Add(assembly);
                }
            }
            return assemblies.ToArray();
        }

        public static Stream GetStream(this StackTrace stackTrace, string name)
        {
            foreach (var assembly in stackTrace.GetAssemblies())
            {
                foreach (var manifestResourceName in assembly.GetManifestResourceNames())
                {
                    if (manifestResourceName.Contains(name))
                    {
                        return assembly.GetManifestResourceStream(manifestResourceName);
                    }
                }
            }
            return null;
        }
    }
}