//
// Copyright (c) 2016 Lou Parslow. Subject to the MIT license, see LICENSE.txt.
//
using System;
using System.IO;
using System.Windows.Media.Media3D;

namespace Node.Net
{
    public interface IFactory { object Create(Type targetType, object source); }
    public interface ILocalToParent { Matrix3D LocalToParent { get; } }
    public interface ILocalToWorld { Matrix3D LocalToWorld { get; } }
}
