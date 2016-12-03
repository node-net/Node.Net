//
// Copyright (c) 2016 Lou Parslow. Subject to the MIT license, see LICENSE.txt.
//
using System.Windows.Media.Media3D;

namespace Node.Net._Model3D
{
    public interface IVisual3DTransformer
    {
        Visual3D GetVisual3D(object value);
    }
}
