using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Deprecated.Readers
{
    public class TextReader : IRead
    {
        public static TextReader Default { get; } = new TextReader();
        public Type DefaultTextType
        {
            get { return defaultTextType; }
            set
            {
                if(defaultTextType != value)
                {
                    if (value == null) throw new ArgumentNullException(nameof(DefaultTextType));
                    if (typeof(IList<string>).IsAssignableFrom(value.GetType())) throw new ArgumentOutOfRangeException(nameof(DefaultTextType), "DefaultTextType must be assignable to System.Collections.Generic.IList<string>");
                    defaultTextType = value;
                }
            }
        }
        private Type defaultTextType = typeof(List<string>);
        public object Read(Stream stream)
        {
            var text = Activator.CreateInstance(DefaultTextType) as IList<string>;
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
