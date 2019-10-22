using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Media3D;

namespace Node.Net.Controls.Extensions
{
    public static class Visual3DExtension
    {
        public static void RemoveChild(DependencyObject parent, Visual3D v3d)
        {
            var parentAsModelVisual3D = parent as ModelVisual3D;
            if (parentAsModelVisual3D != null)
            {
                parentAsModelVisual3D.Children.Remove(v3d);
            }
        }
    }
}
