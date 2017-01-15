using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Readers
{
    public class TextReader : IRead
    {
        public static TextReader Default { get; } = new TextReader();
        public object Read(Stream stream)
        {
            var text = new List<string>();
            string line;
            using (StreamReader reader = new StreamReader(stream))
            {
                while((line = reader.ReadLine()) != null)
                {
                    text.Add(line);
                }
            }
            return text;
        }
        public object Read(string filename) { return IReadExtension.Read(this, filename); }
        public object Read(Type type, string name) { return IReadExtension.Read(this, type, name); }
        public object Read(Assembly assembly, string name) { return IReadExtension.Read(this, assembly, name); }
        public object Open(string openFileDialogFilter = "Text Files (.txt)|*.txt|All Files (*.*)|*.*")
        {
            var ofd = new Microsoft.Win32.OpenFileDialog { Filter = openFileDialogFilter };
            var result = ofd.ShowDialog();
            if (result == true)
            {
                try
                {
                    return Read(ofd.FileName);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Unable to open file {ofd.FileName}", ex);
                }
            }
            return null;
        }
    }
}
