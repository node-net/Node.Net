﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Deprecated.Collections.Internal
{
    interface IFilter { bool? Include(object value); }
}