using System;
using System.Windows.Media.Media3D;

namespace Node.Net.Factory
{
    public interface ITypeName { string TypeName { get; } }
    public interface IFactory { object Create(Type type, object value); }
    public interface IGet { object Get(string name); }
    public interface ITranslation { Vector3D Translation { get; } }
    public interface IRotations { Vector3D RotationsXYZ { get; } }
    public interface ILength { double Length { get; } }
    public interface IAngle { double Angle { get; } }
}
