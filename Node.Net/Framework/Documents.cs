using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Node.Net.Framework
{
    public class Documents : Dictionary<string, dynamic>
    {
        private int maximumCount = 1;
        public int MaximumCount
        {
            get { return maximumCount; }
            set { maximumCount = value; }
        }

        private Type defaultDocumentType = null;
        public Type DefaultDocumentType
        {
            get { return defaultDocumentType; }
            set { defaultDocumentType = value; }
        }

        private string openFileDialogFilter = "Text Files (*.txt)|*.txt|JSON Files (*.json)|*.json|All Files (*.*)|*.*";
        public string OpenFileDialogFilter
        {
            get { return openFileDialogFilter; }
            set { openFileDialogFilter = value; }
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
            openInfo.Invoke(document, parameters);
            if (MaximumCount > 0 && Count == maximumCount)
            { Clear(); }
            Add(name, document);
        }
    }
}
