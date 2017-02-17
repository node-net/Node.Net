//
// Copyright (c) 2016 Lou Parslow. Subject to the Apache 2.0 license, see LICENSE.txt.
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Node.Net
{
    public static class GlobalProperties
    {
        public static Dictionary<string,PerspectiveCamera> PerspectiveCameras { get { return Extensions.PerspectiveCameraExtension.PerspectiveCameras; } }
    }
}
