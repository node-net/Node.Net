using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Media3D;

namespace Node.Net.Beta.Internal.Factories
{
    class Viewport3DFactory : IFactory
    {
        public object Create(Type target_type, object source)
        {
            if (target_type == null) return null;
            if (target_type != typeof(Viewport3D)) return null;
            if (ParentFactory != null)
            {
                var v3d = ParentFactory.Create<Visual3D>(source);
                if(v3d != null)
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
                    viewport.ZoomExtents();
                }

            }
            return null;
        }
        public IFactory ParentFactory { get; set; }
    }
}
