﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Node.Net
{
    public static class Vector3DExtension
    {
        public static Vector3D GetPerpendicular(this Vector3D vector)
        {
            var other = new Vector3D(0, 0, 1);
            if (Vector3D.AngleBetween(vector, other) < 5) other = new Vector3D(0, 1, 0);
            return Vector3D.CrossProduct(other,vector);
        }
    }
}