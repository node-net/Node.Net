using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Node.Net.Factory.Internal
{
    class Transform3DFactory : IFactory
    {
        private IFactory helperFactory = null;

        private IFactory HelperFactory// = new Factories.DefaultFactory();
        {
            get
            {
                if (helperFactory == null) helperFactory = new Matrix3DFactory();
                return helperFactory;
            }
        }
        //private IFactory Factory { get { return Node.Net.Factory.Factory.Default; } }
        public object Create(Type type, object value)
        {
            var matrix3D = HelperFactory.Create<Matrix3D>(value);
            return new MatrixTransform3D { Matrix = matrix3D };

        }
    }
}

