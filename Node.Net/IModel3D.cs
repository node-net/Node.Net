//
// Copyright (c) 2016 Lou Parslow. Subject to the MIT license, see LICENSE.txt.
//
using System.Windows.Media.Media3D;

namespace Node.Net
{
    public interface IModel3D : IChild
    {

        Matrix3D LocalToParent { get; }
    }
}
