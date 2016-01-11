namespace Node.Net.Documents
{
    public class Documents : System.Collections.Generic.Dictionary<string,object>, System.ComponentModel.INotifyPropertyChanged
    {
        public Documents()
        {
            //Types.Add("txt", typeof(TextDocument));
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

        private System.Collections.Generic.Dictionary<string, string> filenames
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
                    NotifyPropertyChanged("CurrentKey");
                }
                Current = this[currentKey];
            }
        }

        private object current = null;
        public object Current
        {
            get { return current; }
            set
            {
                if(!object.ReferenceEquals(current,value))
                {
                    current = value;
                    NotifyPropertyChanged("Current");
                }
            }
        }

        private System.Type defaultDocumentType = null;
        public System.Type DefaultDocumentType
        {
            get
            {
                if(object.ReferenceEquals(null,defaultDocumentType))
                {
                    foreach (string key in Types.Keys)
                    {
                        defaultDocumentType = Types[key];
                    }
                    if (object.ReferenceEquals(null, defaultDocumentType)) defaultDocumentType = typeof(TextDocument);
                }
                return defaultDocumentType;
            }
            set{
                defaultDocumentType = value;
            }
        }

        private string openFileDialogFilter = "Text Files (*.txt)|*.txt|JSON Files (*.json)|*.json|All Files (*.*)|*.*";
        public string OpenFileDialogFilter
        {
            get { return openFileDialogFilter; }
            set { openFileDialogFilter = value; }
        }

        private int maximumCount = -1;
        public int MaximumCount
        {
            get { return maximumCount; }
            set { maximumCount = value; }
        }
        public void New()
        {
            if(MaximumCount > 0 && Count == maximumCount)
            {
                Clear();
            }
            string name = "New Document";
            int index = 2;
            while(ContainsKey(name))
            {
                name = "New Document " + index.ToString();
                ++index;
            }
            Add(name, System.Activator.CreateInstance(DefaultDocumentType));
            CurrentKey = name;
        }

        public void Open()
        {
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.Filter = OpenFileDialogFilter;
            System.Nullable<bool> result = ofd.ShowDialog();
            if (result == true)
            {
                Open(ofd.FileName);
            }
        }

        public virtual void Open(string value)
        {
            char[] delimiters = { '.' };
            string[] parts = value.Split(delimiters,System.StringSplitOptions.RemoveEmptyEntries);
            string extension = parts[parts.Length-1];
            if(Types.ContainsKey(extension))
            {
                System.IO.Stream stream = GetStream(value);
                System.Type[] types = {typeof(System.IO.Stream)};
                System.Reflection.MethodInfo openInfo = Types[extension].GetMethod("Open",types);
                if(!object.ReferenceEquals(null,openInfo))
                {
                    object document = System.Activator.CreateInstance(Types[extension]);
                    object[] parameters = { stream };
                    openInfo.Invoke(document, parameters);

                    if (MaximumCount > 0 && Count == maximumCount)
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
            string filename = "";
            if (filenames.ContainsKey(CurrentKey)) filename = filenames[CurrentKey];

            if (filename.Length == 0) SaveAs();
            else
            {
                Save(filename);
            }
        }

        public void SaveAs()
        {
            Microsoft.Win32.SaveFileDialog ofd = new Microsoft.Win32.SaveFileDialog();
            ofd.Filter = OpenFileDialogFilter;
            System.Nullable<bool> result = ofd.ShowDialog();
            if (result == true)
            {
                Save(ofd.FileName);
            }
        }

        public void Save(string filename) { Save(filename, this); }

        public static System.IO.Stream GetStream(string name)
        {
            return global::Node.Net.IO.StreamExtension.GetStream(name);
            
        }

        public static void Save(string name,object document)
        {
            System.IO.FileInfo fi = new System.IO.FileInfo(name);
            if(!System.IO.Directory.Exists(fi.DirectoryName))
            {
                System.IO.Directory.CreateDirectory(fi.DirectoryName);
            }
            using(System.IO.FileStream fs = new System.IO.FileStream(name,System.IO.FileMode.Create))
            {
                System.Type[] types = { typeof(System.IO.Stream) };
                System.Reflection.MethodInfo saveInfo = document.GetType().GetMethod("Save", types);
                if (!object.ReferenceEquals(null, saveInfo))
                {
                    object[] parameters = { fs };
                    saveInfo.Invoke(document, parameters);
                }
            }
        }
    }
}
