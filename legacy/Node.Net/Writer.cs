using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using System.Xml;

namespace Node.Net
{
    public class Writer : IWrite
    {
        public static Writer Default { get; } = new Writer();

        public void Write(Stream stream, object value)
        {
            if (value != null && WriteFunctions != null)
            {
                foreach (var type in WriteFunctions.Keys)
                {
                    if (type.IsAssignableFrom(value.GetType()))
                    {
                        WriteFunctions[type](stream, value);
                        return;
                    }
                }
            }
            if (value != null)
            {
                if (typeof(ImageSource).IsAssignableFrom(value.GetType()))
                {
                    bitmapSourceWriter.Write(stream, value);
                }
                else
                {
                    if (typeof(DependencyObject).IsAssignableFrom(value.GetType()))
                    {
                        var xmlWriter = XmlWritereate(stream, new XmlWriterSettings
                        {
                            Indent = true
                        });
                        XamlWriter.Save(value, xmlWriter);
                    }
                    else
                    {
                        if (typeof(XmlDocument).IsAssignableFrom(value.GetType()))
                        {
                            (value as XmlDocument).Save(stream);
                        }
                        else
                        {
                            jsonWriter.Write(stream, value);
                        }
                    }
                }
            }
        }
        public void Write(string filename, object value)
        {
            var filestream = new FileStream(filename, FileMode.Create);
            Write(filestream, value);
            filestream.Flush();
            filestream.Close();
            filestream = null;
        }
        public Dictionary<Type, Action<Stream, object>> WriteFunctions { get; set; }
        private JSONWriter jsonWriter = new JSONWriter();
        private BitmapSourceWriter bitmapSourceWriter = new BitmapSourceWriter();
    }
}
