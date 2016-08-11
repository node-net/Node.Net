using System.Windows.Media.Media3D;

namespace Node.Net.Factory
{
    public interface IFactory { T Create<T>(object value); }
    public interface ITranslation { Vector3D Translation { get; } }
    public interface ILength { double Length { get; } }
}
