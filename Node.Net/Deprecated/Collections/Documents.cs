using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;

namespace Node.Net.Deprecated.Collections
{
    public class Documents : Dictionary<string, dynamic>, INotifyPropertyChanged
    {
        private readonly RecentFiles recentFiles = new RecentFiles();
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void NotifyPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
        public int MaximumCount { get; set; } = 1;
        public Type DefaultDocumentType { get; set; } = typeof(Deprecated.Collections.Dictionary);
        public string OpenFileDialogFilter { get; set; } = "JSON Files (*.json)|*.json|All Files (*.*)|*.*";

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
                if (currentKey != value)
                {
                    currentKey = value;
                    NotifyPropertyChanged(nameof(CurrentKey));
                }
            }
        }
        public void Open()
        {
            var ofd = new Microsoft.Win32.OpenFileDialog { Filter = OpenFileDialogFilter };
            var result = ofd.ShowDialog();
            if (result == true)
            {
                Open(ofd.FileName);
            }
        }

        public void Open(string name)
        {
            Open(name, DefaultDocumentType, Node.Net.Extensions.StreamExtension.GetStream(name));
        }

        private void Open(string name, Type documentType, Stream stream)
        {
            var document = Activator.CreateInstance(documentType);


            Type[] types = { typeof(string), typeof(Stream) };
            var openInfo = documentType.GetMethod(nameof(Open), types);
            try
            {
                if (!ReferenceEquals(null, openInfo))
                {
                    object[] parameters = { name, stream };
                    openInfo.Invoke(document, parameters);
                }
                else
                {
                    //types = { typeof(Stream) };
                    Type[] types2 = { typeof(Stream) };
                    openInfo = documentType.GetMethod(nameof(Open), types2);
                    if (!ReferenceEquals(null, openInfo))
                    {
                        object[] parameters = { stream };
                        openInfo.Invoke(document, parameters);

                    }
                }
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show(
                    $"Unable to open {name}{System.Environment.NewLine}{System.Environment.NewLine}{e.Message}",
                    "Error opening file",
                    System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return;
            }

            if (ReferenceEquals(null, openInfo))
            {
                throw new InvalidOperationException($"type {documentType.FullName} does not have public Open(Stream stream) method or public Open(string name,Stream stream) method");
            }
            /*
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
            */
            if (MaximumCount > 0 && Count == MaximumCount)
            { base.Clear(); }
            Add(name, document);
            CurrentKey = name;
        }
    }
}
