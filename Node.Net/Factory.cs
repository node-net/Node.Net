using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net
{
    public class Factory
    {
        private static Factory _default = null;
        public static Factory Default
        {
            get
            {
                if(object.ReferenceEquals(null, _default))
                {
                    _default = new Factory();
                }
                return _default;
            }
        }

        public object Load(Stream stream, string name)
        {
            if(name.Contains(".json"))
            {
                return Json.Reader.Default.Read(stream);
            }
            return null;
        }
    }
}
