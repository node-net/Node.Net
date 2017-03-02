﻿using System.IO;

namespace Node.Net.Deprecated.Data.Writers
{
    public class XmlWriter : IWrite
    {
        public void Write(Stream stream, object value)
        {
            var xdoc = value as System.Xml.XmlDocument;
            if (!object.ReferenceEquals(null, xdoc))
            {
                xdoc.Save(stream);
                return;
            }
            var visual = value as System.Windows.Media.Visual;
            if(!object.ReferenceEquals(null,visual))
            {
                System.Windows.Markup.XamlWriter.Save(value, stream);
                return;
            }
            var dependencyObject = value as System.Windows.DependencyObject;
            if(!object.ReferenceEquals(null,dependencyObject))
            {
                System.Windows.Markup.XamlWriter.Save(value, stream);
                return;
            }
        }
    }
}