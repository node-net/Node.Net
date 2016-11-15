using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Media.Media3D;

namespace Node.Net.Collections
{
    public sealed class GlobalFunctions
    {
        public static Func<IDictionary, Matrix3D> GetLocalToParentFunction { get; set; }
        public static Func<IDictionary, Matrix3D> GetLocalToWorldFunction { get; set; }
        public static Func<IDictionary, Rect3D> GetBoundsFunction { get; set; }
        public static Func<object, Dictionary<string, dynamic>> GetMetaDataFunction { get; set; } = Internal.MetaDataMap.Default.GetMetaData;
    }
}
