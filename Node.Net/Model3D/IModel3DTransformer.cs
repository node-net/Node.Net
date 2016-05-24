using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Node.Net.Model3D
{
    public interface IModel3DTransformer
    {
        System.Windows.Media.Media3D.Model3D GetModel3D(object value);
    }
}
