using System;
using System.Collections;
using System.Windows.Media.Media3D;

namespace Node.Net.Windows
{
    public static class IDictionaryExtension
    {
        public static Model3D GetModel3D(this IDictionary dictionary)
        {
            return IDictionaryModel3DConverter.Default.GetModel3D(dictionary);
        }
    }
}
