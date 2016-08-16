using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Node.Net.Factory.Factories
{
    public class DefaultFactory : Factory
    {
        public DefaultFactory()
        {
            Factories.Add(new StreamSourceFactory { Factory = this });
            var targetTypeFactories = new TargetTypesFactory();
            Factories.Add(targetTypeFactories);
            targetTypeFactories.TargetTypeFactories.Add(typeof(Color), new Internal.ColorFactory());
            targetTypeFactories.TargetTypeFactories.Add(typeof(ITypeName), new Internal.ITypeNameFactory());
            targetTypeFactories.TargetTypeFactories.Add(typeof(ILength), new Internal.ILengthFactory());
            targetTypeFactories.TargetTypeFactories.Add(typeof(IAngle), new Internal.IAngleFactory());
            targetTypeFactories.TargetTypeFactories.Add(typeof(ITranslation), new Internal.ITranslationFactory());
            targetTypeFactories.TargetTypeFactories.Add(typeof(IScale), new Internal.IScaleFactory());
            targetTypeFactories.TargetTypeFactories.Add(typeof(IRotations), new Internal.IRotationsFactory());
            targetTypeFactories.TargetTypeFactories.Add(typeof(Material), new Internal.MaterialFactory());
            targetTypeFactories.TargetTypeFactories.Add(typeof(Matrix3D), new Internal.Matrix3DFactory());
            targetTypeFactories.TargetTypeFactories.Add(typeof(Transform3D), new Internal.Transform3DFactory());
            targetTypeFactories.TargetTypeFactories.Add(typeof(GeometryModel3D), new Internal.GeometryModel3DFactory());
            targetTypeFactories.TargetTypeFactories.Add(typeof(Visual3D), new Internal.Visual3DFactory());
        }
    }
}
