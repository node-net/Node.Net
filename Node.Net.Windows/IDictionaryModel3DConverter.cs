using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.Media3D;

namespace Node.Net.Windows
{
    public sealed class IDictionaryModel3DConverter
    {
        public static IDictionaryModel3DConverter Default { get; } = new IDictionaryModel3DConverter();

        public Model3D GetModel3D(IDictionary dictionary)
        {
            return new Model3DGroup();
        }
    }
}
