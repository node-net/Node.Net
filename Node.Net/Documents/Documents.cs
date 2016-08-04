namespace Node.Net.Documents
{
    public class Documents : System.Collections.Generic.Dictionary<string,object>, System.ComponentModel.INotifyPropertyChanged
    {
        public Documents()
        {
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        protected virtual void NotifyPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(name));
            }
        }

        public System.Collections.Generic.Dictionary<string, System.Type> Types
            = new System.Collections.Generic.Dictionary<string, System.Type>();

        private readonly System.Collections.Generic.Dictionary<string, string> filenames
            = new System.Collections.Generic.Dictionary<string, string>();


        private string currentKey = "";
        public string CurrentKey
        {
            get
            {
                if(!ContainsKey(currentKey)) currentKey = "";
                if(currentKey.Length == 0)
                {
                    foreach (string key in Keys) currentKey = key;
                }
                return currentKey;
            }
            set
            {
                if (currentKey != value)
                {
                    currentKey = value;
                    NotifyPropertyChanged(nameof(CurrentKey));
                }
                Current = this[currentKey];
            }
        }

        private object current;
        public object Current
        {
            get { return current; }
            set
            {
                if(!ReferenceEquals(current,value))
                {
                    current = value;
                    NotifyPropertyChanged(nameof(Current));
                }
            }
        }

        private System.Type defaultDocumentType;
        public System.Type DefaultDocumentType
        {
            get
            {
                if(ReferenceEquals(null,defaultDocumentType))
                {
                    foreach (string key in Types.Keys)
                    {
                        defaultDocumentType = Types[key];
                    }
                    if (ReferenceEquals(null, defaultDocumentType)) defaultDocumentType = typeof(TextDocument);
                }
                return defaultDocumentType;
            }
            set{
                defaultDocumentType = value;
            }
        }
        public string OpenFileDialogFilter { get; set; } = "Text Files (*.txt)|*.txt|JSON Files (*.json)|*.json|All Files (*.*)|*.*";
        public int MaximumCount { get; set; } = -1;
        public void New()
        {
            if(MaximumCount > 0 && Count == MaximumCount)
            {
                Clear();
            }
            var name = "New Document";
            var index = 2;
            while (ContainsKey(name))
            {
                name = "New Document " + index.ToString();
                ++index;
            }
            Add(name, System.Activator.CreateInstance(DefaultDocumentType));
            CurrentKey = name;
        }

        public void Open()
        {
            var ofd = new Microsoft.Win32.OpenFileDialog
            {
                Filter = OpenFileDialogFilter
            };
            var result = ofd.ShowDialog();
            if (result == true)
            {
                Open(ofd.FileName);
            }
        }

        public virtual void Open(string value)
        {
            char[] delimiters = { '.' };
            var parts = value.Split(delimiters,System.StringSplitOptions.RemoveEmptyEntries);
            var extension = parts[parts.Length-1];
            if (Types.ContainsKey(extension))
            {
                var stream = GetStream(value);
                System.Type[] types = {typeof(System.IO.Stream)};
                var openInfo = Types[extension].GetMethod(nameof(Open), types);
                if (!ReferenceEquals(null,openInfo))
                {
                    var document = System.Activator.CreateInstance(Types[extension]);
                    object[] parameters = { stream };
                    openInfo.Invoke(document, parameters);

                    if (MaximumCount > 0 && Count == MaximumCount)
                    {
                        Clear();
                    }

                    Add(value,document);
                    CurrentKey = value;
                }
            }
        }

        public void Save()
        {
            var filename = "";
            if (filenames.ContainsKey(CurrentKey)) filename = filenames[CurrentKey];

            if (filename.Length == 0) SaveAs();
            else
            {
                Save(filename);
            }
        }

        public void SaveAs()
        {
            var ofd = new Microsoft.Win32.SaveFileDialog
            {
                Filter = OpenFileDialogFilter
            };
            var result = ofd.ShowDialog();
            if (result == true)
            {
                Save(ofd.FileName);
            }
        }

        public void Save(string filename) { Save(filename, this); }

        public static System.IO.Stream GetStream(string name)
        {
            return global::Node.Net.Extensions.StreamExtension.GetStream(name);
            
        }

        public static void Save(string name,object document)
        {
            var fi = new System.IO.FileInfo(name);
            if (!System.IO.Directory.Exists(fi.DirectoryName))
            {
                System.IO.Directory.CreateDirectory(fi.DirectoryName);
            }
            using(System.IO.FileStream fs = new System.IO.FileStream(name,System.IO.FileMode.Create))
            {
                System.Type[] types = { typeof(System.IO.Stream) };
                var saveInfo = document.GetType().GetMethod(nameof(Save), types);
                if (!ReferenceEquals(null, saveInfo))
                {
                    object[] parameters = { fs };
                    saveInfo.Invoke(document, parameters);
                }
            }
        }
    }
}
