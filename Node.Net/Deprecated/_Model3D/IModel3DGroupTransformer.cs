//
// Copyright (c) 2016 Lou Parslow. Subject to the MIT license, see LICENSE.txt.
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net._Model3D
{
    public interface IModel3DGroupTransformer
    {
        System.Windows.Media.Media3D.Model3DGroup GetModel3DGroup(object value);
    }
}
