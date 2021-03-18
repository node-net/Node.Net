using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Node.Net
{
    public static class MethodInfoExtension
    {
        public static DirectoryInfo GetLogDirectory(this MethodInfo method)
        {
            var assemblyDir = new FileInfo(method.DeclaringType!.Assembly.Location).Directory;
            if (assemblyDir.FindAncestorWithDirectory(".git") is DirectoryInfo projectDir)
            {
                return new DirectoryInfo(projectDir.FullName + Path.DirectorySeparatorChar + "log");
            }
            return new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) +
                Path.DirectorySeparatorChar + "log");
        }

        public static FileInfo GetLogFileInfo(this MethodInfo method, string extension = ".md")
        {
            return new FileInfo(method.GetLogDirectory().FullName + Path.DirectorySeparatorChar + extension);
        }
    }
}