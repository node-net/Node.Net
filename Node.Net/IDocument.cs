﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net
{
    public interface IDocument : IElement
    {
        string FileName { get; }
    }
}
