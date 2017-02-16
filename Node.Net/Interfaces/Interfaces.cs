using System;
using System.Windows.Media.Media3D;

namespace Node.Net
{
    public interface IFactory { object Create(Type targetType, object source); }
    public interface ILocalToParent { Matrix3D LocalToParent { get; } }
    public interface ILocalToWorld { Matrix3D LocalToWorld { get; } }
}
