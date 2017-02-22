using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Node.Net
{
    public static class ProjectionCameraExtension
    {
        public static Matrix3D GetWorldToLocal(this ProjectionCamera camera) => Beta.Internal.ProjectionCameraExtension.GetWorldToLocal(camera);
        public static Matrix3D GetLocalToWorld(this ProjectionCamera camera) => Beta.Internal.ProjectionCameraExtension.GetLocalToWorld(camera);
    }
}
