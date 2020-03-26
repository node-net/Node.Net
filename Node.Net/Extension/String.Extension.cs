using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Node.Net
{
    public static class StringExtension
    {
        public static bool IsFileDialogFilter(this string value)
        {
            return value.Contains('|');
        }

        public static bool IsValidFileName(this string value)
        {
            if (value.Contains('\\'))
            {
                if (value.IndexOfAny(Path.GetInvalidPathChars()) >= 0)
                {
                    return false;
                }

                string[]? parts = value.Split('\\');
                if (parts[parts.Length - 1].IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
                {
                    return false;
                }
            }
            else
            {
                if (value.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
                {
                    return false;
                }
            }
            return true;
        }

        public static double GetMeters(this string value) => Length.GetMeters(value);

        public static double GetRawValue(this string value)
        {
            try
            {
                string[]? words = value.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (words.Length > 0)
                {
                    string? word = words[0].Replace("'", "");
                    return Convert.ToDouble(word);
                }
                return 0.0;
            }
            catch
            {
                return 0.0;
            }
        }

        public static Stream GetStream(this string value)
        {
            if (File.Exists(value))
            {
                return new FileStream(value, FileMode.Open, FileAccess.Read);
            }
            StackTrace? stackTrace = new StackTrace();
            foreach (System.Reflection.Assembly? assembly in stackTrace.GetAssemblies())
            {
                Stream? stream = assembly.GetStream(value);
                if (stream != null) { return stream; }
            }

            return new MemoryStream(Encoding.UTF8.GetBytes(value));
        }
    }
}