using System.Collections;
using System.Windows.Media.Media3D;

namespace Node.Net.Factories.Deprecated.Factories.Helpers
{
    public static class IRotationsHelper
    {
        class ConcreteRotations : IRotations { public Vector3D RotationsXYZ { get; set; } }
        public static IRotations FromIDictionary(IDictionary source, IFactory factory)
        {
            var concreteRotations = new ConcreteRotations
            {
                RotationsXYZ = new Vector3D(
                    IDictionaryHelper.GetAngleDegrees(source, "RotationX,Spin,Roll", factory),
                    IDictionaryHelper.GetAngleDegrees(source, "RotationY,Tilt,Pitch", factory),
                    IDictionaryHelper.GetAngleDegrees(source, "RotationZ,Orientation,Yaw", factory))
            };
            return concreteRotations;
        }
    }
}
