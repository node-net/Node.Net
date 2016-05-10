﻿using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Node.Net.Model3D
{
    public class MaterialHelper
    {
        public static Material GetImageMaterial(ImageSource imageSource,Brush specularBrush = null,double specularPower=10)
        {
            MaterialGroup material = new MaterialGroup();
            DiffuseMaterial diffuse = new DiffuseMaterial()
            {
                Brush = new ImageBrush()
                {
                    ImageSource = imageSource,
                    TileMode = TileMode.Tile
                }
            };
            material.Children.Add(diffuse);
            if (!object.ReferenceEquals(null, specularBrush))
            {
                SpecularMaterial specular = new SpecularMaterial()
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