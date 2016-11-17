using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Node.Net.Extensions
{
    public static class ProjectionCameraExtension
    {
        public static void SetDirection(ProjectionCamera target, ProjectionCamera source)
        {
            target.LookDirection = source.LookDirection;
            target.UpDirection = source.UpDirection;
        }

    }
}
