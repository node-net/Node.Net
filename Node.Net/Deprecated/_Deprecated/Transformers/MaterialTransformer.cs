﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Node.Net.Deprecated.Transformers
{
    public class MaterialTransformer : TypeTransformer
    {
        public override object Transform(object item)
        {
            if (ReferenceEquals(null, item)) return null;
            if (item.GetType() == typeof(string)) return Transform(item.ToString());
            return null;
        }

        public static Material Transform(string name)
        {
            var brush_transformer = new BrushTransformer();
            return new DiffuseMaterial(brush_transformer.Transform(name));
        }

        public static Material Transform(IDictionary dictionary)
        {
            return null;
        }
    }
}