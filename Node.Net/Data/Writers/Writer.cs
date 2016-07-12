using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Data.Writers
{
    public class Writer
    {
        private Dictionary<Type, IWrite> writersMap;
        public Dictionary<Type, IWrite> WritersMap
        {
            get
            {
                if(writersMap == null)
                {
                    writersMap = new Dictionary<Type, IWrite>();
                    writersMap.Add(typeof(IEnumerable), new JsonWriter());
                }
                return writersMap;
            }
        }
        public void Write(Stream stream, object value)
        {
            if (value == null) return;

        }
    }
}
