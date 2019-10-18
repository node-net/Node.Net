using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Node.Net
{
    public class IDictionarySpatialConverter
    {
        public static IDictionarySpatialConverter Default = new IDictionarySpatialConverter();

        public Math.Matrix3D GetLocalToParent(IDictionary dictionary)
        {
            var localToParent = new Math.Matrix3D();
            localToParent.Translate(GetTranslation(dictionary));
            return localToParent;
        }

        public Math.Vector3D GetTranslation(IDictionary dictionary)
        {
            return new Math.Vector3D(
                dictionary.Get<double>("X"),
                dictionary.Get<double>("Y"),
                dictionary.Get<double>("Z")
                );
        }
    }
}
