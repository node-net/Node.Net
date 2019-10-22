using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using static System.Environment;

namespace Node.Net.Diagnostics
{
    public static class FileSystem
    {
        public static string TempDir
        {
            get
            {
                var dev_root = GetEnvironmentVariable("DEV_ROOT");
                if(Directory.Exists(dev_root))
                {
                    return $"{dev_root}\\tmp".Replace("\\\\", "\\");
                }
                return Path.GetTempPath();
            }
        }
        public static void Delete(string directory)
        {
            if (Directory.Exists(directory))
            {
                DeleteFiles(directory);
                Directory.Delete(directory, true);
            }
        }
        public static void DeleteFiles(string directory)
        {
            if (Directory.Exists(directory))
            {
                File.SetAttributes(directory, FileAttributes.Normal);
                foreach (string subdir in Directory.GetDirectories(directory))
                {
                    DeleteFiles(subdir);
                }
                var filenames = Directory.GetFiles(directory);
                foreach (var filename in filenames)
                {
                    File.SetAttributes(filename, FileAttributes.Normal);
                    File.Delete(filename);
                }
            }
        }
    }
}
