using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;

namespace Node.Net.Framework
{
    public class Documents : Dictionary<string, dynamic>, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void NotifyPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        private int maximumCount = 1;
        public int MaximumCount
        {
            get { return maximumCount; }
            set { maximumCount = value; }
        }

        private Type defaultDocumentType = typeof(Collections.Dictionary);
        public Type DefaultDocumentType
        {
            get { return defaultDocumentType; }
            set { defaultDocumentType = value; }
        }

        private string openFileDialogFilter = "JSON Files (*.json)|*.json|All Files (*.*)|*.*";
        public string OpenFileDialogFilter
        {
            get { return openFileDialogFilter; }
            set { openFileDialogFilter = value; }
        }

        public new void Clear()
        {
            base.Clear();
            CurrentKey = "";
        }
        private string currentKey = "";
        public string CurrentKey
        {
            get { return currentKey; }
            set
            {
                if(currentKey != value)
                {
                    currentKey = value;
                    NotifyPropertyChanged("CurrentKey");
                }
            }
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

        public void Open(string name)
        {
            Open(name, DefaultDocumentType, IO.StreamExtension.GetStream(name));
        }

        private void Open(string name, Type documentType, Stream stream)
        {
            object document = Activator.CreateInstance(documentType);
            Type[] types = { typeof(Stream) };
            MethodInfo openInfo = documentType.GetMethod("Open", types);
            if (object.ReferenceEquals(null, openInfo))
            {
                throw new InvalidOperationException($"type {documentType.FullName} does not have public Open(Stream stream) method");
            }
            object[] parameters = { stream };

            try {
                openInfo.Invoke(document, parameters);
            }
            catch(Exception e)
            {
                System.Windows.MessageBox.Show(
                    $"Unable to open {name}{System.Environment.NewLine}{System.Environment.NewLine}{e.Message}",
                    "Error opening file",
                    System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return;
            }
            if (MaximumCount > 0 && Count == maximumCount)
            { base.Clear(); }
            Add(name, document);
            CurrentKey = name;
        }
    }
}
