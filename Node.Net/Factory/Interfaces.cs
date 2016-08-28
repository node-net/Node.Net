using System;
using System.Collections;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Node.Net.Factory
{
    public interface ITypeName { string TypeName { get; } }
    public interface IFactory { object Create(Type targetType, object source); }
    public interface IChild {  IFactory Parent { get; set; } }
    public interface IChildFactory : IFactory, IChild { }
    public interface IFactoryAdapter : IChildFactory { string Name { get; } }
    public interface IHelperFactory { IFactory HelperFactory { get; set; } }
    public interface ITargetType { Type TargetType { get; } }
    public interface ISourceType { Type SourceType { get; } }
    public interface ITypeFactory<T> { T Create(object source); Type TargetType { get; } }
    public interface ICompositeFactory : IChildFactory, IDictionary { }
    public interface IGet { object Get(string name); }
    public interface ITranslation { Vector3D Translation { get; } }
    public interface IRotations { Vector3D RotationsXYZ { get; } }
    public interface IScale { Vector3D Scale { get; } }
    public interface IMatrix3D {  Matrix3D Matrix3D { get;} }
    public interface ILength { double Length { get; } }
    public interface IAngle { double Angle { get; } }
    public interface IColor { Color Color { get; } }
    public interface IPrimaryModel { Model3D Model3D { get; } }
}
