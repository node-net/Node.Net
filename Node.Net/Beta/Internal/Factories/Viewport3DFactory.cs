using System;
using System.Windows.Controls;
using System.Windows.Media.Media3D;

namespace Node.Net.Beta.Internal.Factories
{
    class Viewport3DFactory : IFactory
    {
        public object Create(Type targetType, object source)
        {
            if (targetType == null) return null;
            if (targetType != typeof(Viewport3D)) return null;
            if (ParentFactory != null)
            {
                var v3d = ParentFactory.Create<Visual3D>(source);
                if (v3d != null)
                {
                    var viewport = new Viewport3D
                    {
                        Camera = new OrthographicCamera
                        {
                            Position = new Point3D(0, 0, 0),
                            LookDirection = new Vector3D(0, 0, -1),
                            UpDirection = new Vector3D(0, 1, 0),
                            NearPlaneDistance = 0,
                            FarPlaneDistance = double.PositiveInfinity,
                            Width = 50
                        }
                    };
                    viewport.Children.Add(v3d);
                    (viewport.Camera as ProjectionCamera)?.ZoomExtents(v3d.FindBounds(), 500, 500);
                    return viewport;
                }

            }
            return null;
        }
        public IFactory ParentFactory { get; set; }
    }
}
