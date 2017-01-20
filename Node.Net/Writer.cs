//
// Copyright (c) 2016 Lou Parslow. Subject to the Apache 2.0 license, see LICENSE.txt.
//
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net
{
    public sealed class Writer
    {
        public static Writer Default { get; } = new Writer();
        public void Write(Stream stream, object value) { writer.Write(stream, value); }
        public void Write(string filename,object value){ writer.Write(filename, value); }
        public void Save(object value,string saveAsDialogFilter = "JSON Files (.json)|*.json|All Files (*.*)|*.*") { writer.Save(value, saveAsDialogFilter); }
        public void SaveAs(object value,string saveAsDialogFilter = "JSON Files (.json)|*.json|All Files (*.*)|*.*") { writer.SaveAs(value, saveAsDialogFilter); }
        private global::Node.Net.Writers.Writer writer = new global::Node.Net.Writers.Writer();
    }
}
