﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Deprecated.Writers
{
    public interface IWrite
    {
        void Write(Stream stream,object value);
    }
}