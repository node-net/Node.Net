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
                    Node.Net.Factories.Extension.IDictionaryExtension.GetAngleDegrees(source, "RotationX,Spin,Roll"),
                    Node.Net.Factories.Extension.IDictionaryExtension.GetAngleDegrees(source, "RotationY,Tilt,Pitch"),
                    Node.Net.Factories.Extension.IDictionaryExtension.GetAngleDegrees(source, "RotationZ,Orientation,Yaw"))
            };
            return concreteRotations;
        }
    }
}
