using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Node.Net.Model3D
{
    public interface IVisual3DTransformer
    {
        Visual3D GetVisual3D(object value);
    }
}
