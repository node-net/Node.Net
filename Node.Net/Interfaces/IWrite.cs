﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Interfaces
{
    public interface IWrite
    {
        void Write(Stream stream, object value);
    }
}
