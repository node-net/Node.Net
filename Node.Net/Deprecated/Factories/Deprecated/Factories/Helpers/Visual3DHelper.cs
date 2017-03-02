﻿using System.Collections;
using System.Windows.Media.Media3D;

namespace Node.Net.Factories.Deprecated.Factories.Helpers
{
    public static class Visual3DHelper
    {
        public static Visual3D FromIDictionary(IDictionary source, IFactory factory)
        {
            if (source == null) return null;
            var model3D = factory.Create<Model3D>(source,null);
            if (model3D != null)
            {
                return new ModelVisual3D { Content = model3D };
            }
            return null;
        }
    }
}