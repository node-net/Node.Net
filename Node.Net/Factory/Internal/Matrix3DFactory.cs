using System.Windows.Media.Media3D;

namespace Node.Net.Factory.Internal
{
    class Matrix3DFactory : IFactory
    {
        public T Create<T>(object value)
        {
            var matrix3D = new Matrix3D();
            var translation = ITranslationFactory.Default.Create<ITranslation>(value);
            matrix3D.Translate(translation.Translation);
            return (T)(object)matrix3D;
        }


    }
}
