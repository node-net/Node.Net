using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;

namespace Node.Net
{
    public class Element : Dictionary<string, dynamic>, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName]string caller = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(caller));
            }
        }
        protected bool SetField<T>(ref T field,T value, [CallerMemberName] string propertyName=null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
        public string Name
        {
            get
            {
                var key = global::Node.Net.Extension.GetKey(this);
                if (key != null) return key.ToString();
                return string.Empty;
            }
        }
        [Browsable(false)]
        public string JSON
        {
            get
            {
                var writer = new global::Node.Net.Writers.JsonWriter { Format = Writers.JsonFormat.Indented };
                using (MemoryStream memory = new MemoryStream())
                {
                    writer.Write(memory, this);
                    memory.Flush();
                    memory.Seek(0, SeekOrigin.Begin);
                    using (StreamReader sr = new StreamReader(memory))
                    {
                        return sr.ReadToEnd();
                    }
                }
            }
        }
        [Browsable(false)]
        public new IEqualityComparer<string> Comparer { get { return base.Comparer; } }
        [Browsable(false)]
        public new int Count { get { return base.Count; } }
        [Browsable(false)]
        public new Dictionary<string, dynamic>.KeyCollection Keys { get { return base.Keys; } }
        [Browsable(false)]
        public new Dictionary<string, dynamic>.ValueCollection Values { get { return base.Values; } }
    }
}
