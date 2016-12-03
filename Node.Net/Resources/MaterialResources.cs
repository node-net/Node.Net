//
// Copyright (c) 2016 Lou Parslow. Subject to the MIT license, see LICENSE.txt.
//
using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Node.Net.Resources
{
    public class MaterialResources : Resources
    {
        public IResources ImageResources;

        protected override object GetDynamicResource(string name)
        {
            if(IsKnownColor(name))
            {
                var material = new DiffuseMaterial(new SolidColorBrush((Color)ColorConverter.ConvertFromString(name)));
                Add(name, material);
                return material;
            }

            if(ImageResources != null)
            {
                var imageSource = ImageResources.GetResource(name) as ImageSource;
                if (imageSource == null) imageSource = ImageResources.GetResource($"{name}.jpg") as ImageSource;
                if(imageSource != null)
                {
                    var material = _Model3D.MaterialHelper.GetImageMaterial(imageSource);
                    Add(name, material);
                    return material;
                }
            }
            return base.GetDynamicResource(name);
        }

        private static List<string> _knownColors;
        private static List<string> KnownColors
        {
            get
            {
                if(_knownColors == null)
                {
                    _knownColors = new List<string>();
                    var properties = typeof(Colors).GetProperties();
                    foreach(var property in properties)
                    {
                        _knownColors.Add(property.Name);
                    }
                }
                return _knownColors;
            }
        }
        public static bool IsKnownColor(string name)
        {
            try
            {
                if (KnownColors.Contains(name))
                {
                    return true;
                }
            }
            catch (Exception exception)
            {
                throw new Exception($"{typeof(MaterialResources).FullName}.IsKnownColor('{name}')", exception);
            }

            return false;
        }
    }
}
