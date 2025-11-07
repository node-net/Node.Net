#if IS_WINDOWS
using System;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Node.Net.Internal
{
    internal class MaterialFactory
    {
        public object Create(Type targetType, object source)
        {
            if (targetType == null)
            {
#pragma warning disable CS8603 // Possible null reference return.
                return null;
#pragma warning restore CS8603 // Possible null reference return.
            }

            if (!typeof(Material).IsAssignableFrom(targetType))
            {
#pragma warning disable CS8603 // Possible null reference return.
                return null;
#pragma warning restore CS8603 // Possible null reference return.
            }

            if (source != null && source is Brush)
            {
                return new DiffuseMaterial(source as Brush);
            }
            if (ParentFactory != null)
            {
                Brush? brush = ParentFactory.Create<Brush>(source);
                if (brush != null)
                {
                    return new DiffuseMaterial(brush);
                }
            }
#pragma warning disable CS8603 // Possible null reference return.
            return null;
#pragma warning restore CS8603 // Possible null reference return.
        }

        public IFactory ParentFactory { get; set; }
    }
}
#endif