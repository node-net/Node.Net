﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net
{
    public class Factory : IFactory
    {
        private static IFactory _default = null;
        public static IFactory Default
        {
            get
            {
                if(object.ReferenceEquals(null, _default))
                {
                    _default = new Factory();
                }
                return _default;
            }
            set
            {
                _default = value;
            }
        }

        private IDictionary _resources = new Dictionary<string, dynamic>();
        public IDictionary Resources
        {
            get { return _resources; }
            set { _resources = value; }
        }
        public object Load(Stream stream, string name)
        {
            if(name.Contains(".json"))
            {
                return Json.Reader.Default.Read(stream);
            }
            return null;
        }

        public void Save(object item,Stream stream)
        {

        }

        public object Transform(object item,Type type)
        {
            return null;
        }
    }
}