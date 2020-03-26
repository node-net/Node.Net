using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using System.Xml;

namespace Node.Net
{
    public enum JsonFormat { Compact = 0, Pretty = 1 }

    public class Writer : IWrite
    {
        public void Write(Stream stream, object value)
        {
            if (value != null && WriteFunctions != null)
            {
                foreach (Type? type in WriteFunctions.Keys)
                {
                    if (type.IsInstanceOfType(value))
                    {
                        WriteFunctions[type](stream, value);
                        return;
                    }
                }
            }
            if (value != null)
            {
                if (value is ImageSource)
                {
                    bitmapSourceWriter.Write(stream, value);
                }
                else
                {
                    if (value is DependencyObject)
                    {
                        XmlWriter? xmlWriter = XmlWriter.Create(stream, new XmlWriterSettings
                        {
                            Indent = true
                        });
                        XamlWriter.Save(value, xmlWriter);
                    }
                    else
                    {
                        if (value is XmlDocument)
                        {
                            (value as XmlDocument)?.Save(stream);
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
            FileStream? filestream = new FileStream(filename, FileMode.Create);
            Write(filestream, value);
            filestream.Flush();
            filestream.Close();
        }

        public JsonFormat JsonFormat
        {
            get { return (JsonFormat)(int)jsonWriter.Format; }
            set { jsonWriter.Format = (Internal.JSONFormat)(int)value; }
        }

        public string IndentString
        {
            get { return jsonWriter.IndentString; }
            set { jsonWriter.IndentString = value; }
        }

        public Dictionary<Type, Action<Stream, object>> WriteFunctions { get; set; } = new Dictionary<Type, Action<Stream, object>>();
        private readonly Internal.JsonWriter jsonWriter = new Internal.JsonWriter();
        private readonly Internal.BitmapSourceWriter bitmapSourceWriter = new Internal.BitmapSourceWriter();
    }
}