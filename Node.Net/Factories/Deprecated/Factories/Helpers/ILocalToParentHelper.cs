using System.Collections;
using System.Windows.Media.Media3D;

namespace Node.Net.Factories.Deprecated.Factories.Helpers
{
    static class ILocalToParentHelper
    {
        private static IFactory transformFactory = Helpers.IFactoryHelper.CreateTransformFactory();
        class ConcreteLocalToParent : ILocalToParent { public Matrix3D LocalToParent { get; set; } = new Matrix3D(); }
        public static ILocalToParent FromIDictionary(IDictionary dictionary, IFactory factory)
        {
            if (dictionary == null) throw new System.NullReferenceException("IDictionary dictionary is null");
            if (factory == null) throw new System.NullReferenceException("IFactory factory is null");
            IMatrix3D imatrix = IMatrix3DHelper.FromIDictionary(dictionary, transformFactory);
            if (imatrix != null)
            {
                return new ConcreteLocalToParent { LocalToParent = imatrix.Matrix3D };
            }
            return new ConcreteLocalToParent();
        }
    }
}
