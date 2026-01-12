#if IS_WINDOWS
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
                object? matrix3D = ParentFactory.Create(typeof(Matrix3D), source);
                if (matrix3D != null)
                {
                    return new MatrixTransform3D { Matrix = (Matrix3D)matrix3D };
                }
            }

#pragma warning disable CS8603 // Possible null reference return.
            return null;
#pragma warning restore CS8603 // Possible null reference return.
        }

        public IFactory? ParentFactory { get; set; }
    }
}
#endif