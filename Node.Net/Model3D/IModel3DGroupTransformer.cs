using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Model3D
{
    public interface IModel3DGroupTransformer
    {
        System.Windows.Media.Media3D.Model3DGroup GetModel3DGroup(object value);
    }
}
