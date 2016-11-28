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
    public class Writer
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
    }
}
