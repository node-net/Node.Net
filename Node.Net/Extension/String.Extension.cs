using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net
{
    public static class StringExtension
    {
        public static bool IsFileDialogFilter(this string value)
        {
            if (!value.Contains('|')) return false;
            return true;
        }

        public static bool IsValidFileName(this string value)
        {
            if (value.Contains('\\'))
            {
                if (value.IndexOfAny(Path.GetInvalidPathChars()) >= 0) return false;
                var parts = value.Split('\\');
                if (parts[parts.Length - 1].IndexOfAny(Path.GetInvalidFileNameChars()) >= 0) return false;
            }
            else
            {
                if (value.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0) return false;
            }
            return true;
        }
        public static double GetMeters(this string value) => Beta.Internal.Units.Length.GetMeters(value);
    }
}
