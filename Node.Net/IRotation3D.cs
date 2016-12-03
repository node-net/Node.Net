﻿//
// Copyright (c) 2016 Lou Parslow. Subject to the MIT license, see LICENSE.txt.
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Node.Net
{
    public interface IRotation3D
    {
        Quaternion Rotation3D { get; }
    }
}
