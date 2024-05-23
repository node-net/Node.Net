using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Node.Net
{
    public static class DirectoryInfoExtension
    {
        public static IEnumerable<DirectoryInfo> GetAncestors(this DirectoryInfo dir)
        {
            List<DirectoryInfo> ancestors = new List<DirectoryInfo>();
            var cur = dir;
            while (cur != null)
            {
                if (cur.Parent is DirectoryInfo parent)
                {
                    ancestors.Add(parent);
                    cur = parent;
                }
                else
                {
                    cur = null;
                }
            }
            return ancestors;
        }

        public static DirectoryInfo FindAncestorWithDirectory(this DirectoryInfo dir, string search_pattern)
        {
            return dir.GetAncestors().Where(di => di.GetDirectories(search_pattern).Length > 0).FirstOrDefault();
        }

        public static DirectoryInfo FindAncestorWithFile(this DirectoryInfo dir, string search_pattern)
        {
            return dir.GetAncestors().Where(di => di.GetFiles(search_pattern).Length > 0).FirstOrDefault();
        }
    }
}