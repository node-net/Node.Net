using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Readers
{
    public static class StackTraceExtension
    {
        public static Assembly[] GetAssemblies(this StackTrace stackTrace)
        {
            var assemblies = new List<Assembly>();
            for(int f = 1; f < stackTrace.FrameCount; ++f)
            {
                var assembly = stackTrace.GetFrame(f).GetMethod().DeclaringType.Assembly;
                if(!assemblies.Contains(assembly))
                {
                    assemblies.Add(assembly);
                }
            }
            return assemblies.ToArray();
        }
        /*
         * var stackTrace = new StackTrace();
                var callingMethod = stackTrace.GetFrame(1).GetMethod();
                var callingType = callingMethod.DeclaringType;
                var stream = callingType.Assembly.GetStream(filename);
                if(stream != null)
                {
                    var item = read.Read(stream);
                    SetPropertyValue(item, "FileName", filename);
                   
         * 
         */
    }
}
