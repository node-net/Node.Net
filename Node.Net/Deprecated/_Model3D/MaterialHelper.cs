//
// Copyright (c) 2016 Lou Parslow. Subject to the MIT license, see LICENSE.txt.
//
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Node.Net._Model3D
{
    public class MaterialHelper
    {
        public static Material GetImageMaterial(ImageSource imageSource,Brush specularBrush = null,double specularPower=10)
        {
            var material = new MaterialGroup();
            var diffuse = new DiffuseMaterial
            {
                Brush = new ImageBrush
                {
                    ImageSource = imageSource,
                    TileMode = TileMode.Tile
                }
            };
            material.Children.Add(diffuse);
            if (!ReferenceEquals(null, specularBrush))
            {
                var specular = new SpecularMaterial
                {
                    Brush = specularBrush,
                    SpecularPower = specularPower
                };
                material.Children.Add(specular);
            }
            return material;
        }


    }
}
