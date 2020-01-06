using System;
using System.Windows.Media.Media3D;

namespace Node.Net.Internal
{
    internal sealed class Transform3DFactory : IFactory
    {
        public object Create(Type targetType, object source)
        {
            if (ParentFactory != null)
            {
                var matrix3D = ParentFactory.Create(typeof(Matrix3D), source);
                if (matrix3D != null)
                {
                    return new MatrixTransform3D { Matrix = (Matrix3D)matrix3D };
                }
            }

            return null;
        }

        public IFactory ParentFactory { get; set; }
    }
}