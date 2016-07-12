using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml;

namespace Node.Net.Data.Writers
{
    public class Writer : IWrite
    {
        private Dictionary<Type, IWrite> writersMap;
        public Dictionary<Type, IWrite> WritersMap
        {
            get
            {
                if (writersMap == null)
                {
                    writersMap = new Dictionary<Type, IWrite>();
                    var primitiveWriter = new PrimitiveWriter();
                    writersMap.Add(typeof(string), primitiveWriter);
                    writersMap.Add(typeof(double), primitiveWriter);
                    writersMap.Add(typeof(bool), primitiveWriter);
                    var xmlWriter = new Writers.XmlWriter();
                    writersMap.Add(typeof(XmlDocument), xmlWriter);
                    writersMap.Add(typeof(BitmapSource), new BitmapImageWriter());
                    writersMap.Add(typeof(Visual), xmlWriter);
                    writersMap.Add(typeof(IEnumerable), new JsonWriter());
                }
                return writersMap;
            }
        }
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
    }
}
