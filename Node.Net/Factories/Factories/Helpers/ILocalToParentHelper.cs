using System.Collections;
using System.Windows.Media.Media3D;

namespace Node.Net.Factories.Factories.Helpers
{
    static class ILocalToParentHelper
    {
        class ConcreteLocalToParent : ILocalToParent { public Matrix3D LocalToParent { get; set; } = new Matrix3D(); }
        public static ILocalToParent FromIDictionary(IDictionary dictionary, IFactory factory)
        {
            IMatrix3D imatrix = IMatrix3DHelper.FromIDictionary(dictionary, factory);
            if (imatrix != null)
            {
                return new ConcreteLocalToParent { LocalToParent = imatrix.Matrix3D };
            }
            return new ConcreteLocalToParent();
        }
    }
}
