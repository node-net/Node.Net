using System;
using System.Windows.Media.Media3D;

namespace Node.Net.Factory
{
    public interface ITypeName { string TypeName { get; } }
    public interface IFactory { object Create(Type targetType, object source); }
    public interface IGet { object Get(string name); }
    public interface ITranslation { Vector3D Translation { get; } }
    public interface IRotations { Vector3D RotationsXYZ { get; } }
    public interface IScale { Vector3D Scale { get; } }
    public interface ILength { double Length { get; } }
    public interface IAngle { double Angle { get; } }
}
