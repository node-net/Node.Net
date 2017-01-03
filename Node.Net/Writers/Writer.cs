using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml;

namespace Node.Net.Writers
{
    public class Writer : IWrite
    {
        public static Writer Default { get; } = new Writer();
        public void Write(Stream stream, object value)
        {
            if (value == null) return;
            foreach (var type in WritersMap.Keys)
            {
                if (type.IsAssignableFrom(value.GetType()))
                {
                    WritersMap[type].Write(stream, value);
                    break;
                }
            }
        }
        public void Write(string filename, object value) => IWriteExtension.Write(this, filename, value);

        private Dictionary<Type, IWrite> writersMap;
        public Dictionary<Type, IWrite> WritersMap
        {
            get
            {
                if (writersMap == null)
                {
                    writersMap = new Dictionary<Type, IWrite>();
                    var xmlWriter = new Writers.XmlWriter();
                    writersMap.Add(typeof(XmlDocument), xmlWriter);
                    writersMap.Add(typeof(BitmapSource), new BitmapSourceWriter());
                    writersMap.Add(typeof(Visual), xmlWriter);
                    writersMap.Add(typeof(DependencyObject), xmlWriter);
                    writersMap.Add(typeof(IEnumerable), new JsonWriter());
                }
                return writersMap;
            }
            set { writersMap = value; }
        }

        public void Save(object value,string saveFileDialogFilter = "JSON Files (.json)|*.json|All Files (*.*)|*.*")
        {
            var filename = GetPropertyValue(value, "FileName");
            if(filename.Length == 0 || filename.IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
            {
                SaveAs(value, saveFileDialogFilter);
            }
            else
            {
                try
                {
                    Write(filename, value);
                }
                catch(Exception ex)
                {
                    throw new Exception($"Unable to Save '{filename}'", ex);
                }
            }
        }

        public void SaveAs(object value,string saveFileDialogFilter = "JSON Files (.json)|*.json|All Files (*.*)|*.*")
        {
            var sfd = new Microsoft.Win32.SaveFileDialog { Filter = saveFileDialogFilter };
            var result = sfd.ShowDialog();
            if(result == true)
            {
                try
                {
                    Write(sfd.FileName, value);
                    SetPropertyValue(value, "FileName", sfd.FileName);
                }
                catch(Exception ex)
                {
                    throw new Exception($"Unable to SaveAs '{sfd.FileName}'",ex);
                }
            }
        }

        private static string GetPropertyValue(object item, string propertyName)
        {
            if (item != null)
            {
                var propertyInfo = item.GetType().GetProperty(propertyName);
                if (propertyInfo != null)
                {
                    var value = propertyInfo.GetValue(item);
                    if (value != null) return value.ToString();
                }
            }
            return string.Empty;
        }
        private static void SetPropertyValue(object item, string propertyName, object propertyValue)
        {
            if (item != null)
            {
                var propertyInfo = item.GetType().GetProperty(propertyName);
                if (propertyInfo != null)
                {
                    propertyInfo.SetValue(item, propertyValue);
                }
            }
        }
    }
}
