using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Node.Net
{
    public class Writer
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
                else { jsonWriter.Write(stream, value); }
            }
        }
        public Dictionary<Type, Action<Stream, object>> WriteFunctions { get; set; }
        private JSONWriter jsonWriter = new JSONWriter();
        private BitmapSourceWriter bitmapSourceWriter = new BitmapSourceWriter();
    }
}
