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
            List<Assembly>? assemblies = new List<Assembly>();
            for (int f = 1; f < stackTrace.FrameCount; ++f)
            {
                Assembly? assembly = stackTrace.GetFrame(f).GetMethod().DeclaringType.Assembly;
                if (!assemblies.Contains(assembly))
                {
                    assemblies.Add(assembly);
                }
            }
            return assemblies.ToArray();
        }

        public static Stream GetStream(this StackTrace stackTrace, string name)
        {
            foreach (Assembly? assembly in stackTrace.GetAssemblies())
            {
                foreach (string? manifestResourceName in assembly.GetManifestResourceNames())
                {
                    if (manifestResourceName.Contains(name))
                    {
#pragma warning disable CS8603 // Possible null reference return.
                        return assembly.GetManifestResourceStream(manifestResourceName);
#pragma warning restore CS8603 // Possible null reference return.
                    }
                }
            }
#pragma warning disable CS8603 // Possible null reference return.
            return null;
#pragma warning restore CS8603 // Possible null reference return.
        }
    }
}