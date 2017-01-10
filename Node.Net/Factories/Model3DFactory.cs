﻿using System;
using System.Collections;
using System.Windows.Media.Media3D;

namespace Node.Net.Factories
{
    public sealed class Model3DFactory : Generic.TargetTypeFactory<Model3D>
    {
        public Func<IDictionary,Model3D> PrimaryModel3DHelperFunction { get; set; }
        public override Model3D Create(object source)
        {
            var instance = CreateFromDictionary(source as IDictionary);
            if (instance != null) return instance;

            if (Helper != null)
            {
                var geometry3D = Helper.Create(typeof(GeometryModel3D), source) as GeometryModel3D;
                if (geometry3D != null) return geometry3D;
            }
            return null;
        }

        private Model3D CreateFromDictionary(IDictionary source)
        {
            if (source == null) return null;
            Model3DGroup model3DGroup = null;
            var primaryModel = GetPrimaryModel3D(source);
            if (primaryModel != null)
            {
                model3DGroup = new Model3DGroup { Transform = GetTransform3D(source) };
                model3DGroup.Children.Add(primaryModel);
            }
            
            foreach(var key in source.Keys)
            {
                var child_dictionary = source[key] as IDictionary;
                if(child_dictionary != null)
                {
                    var child_model = Create(child_dictionary);
                    if (child_model != null)
                    {
                        if(model3DGroup == null)
                        {
                            model3DGroup = new Model3DGroup { Transform = GetTransform3D(source) };
                        }
                        model3DGroup.Children.Add(child_model);
                    }
                }
            }
            return model3DGroup;
        }

        private Model3D GetPrimaryModel3D(IDictionary source)
        {
            if(PrimaryModel3DHelperFunction != null)
            {
                var model = PrimaryModel3DHelperFunction(source);
                if (model != null) return model;
            }
            if (Helper != null)
            {
                var geometry3D = Helper.Create(typeof(GeometryModel3D), source) as GeometryModel3D;
                if (geometry3D != null) return geometry3D;
            }
            return null;
        }
        private Transform3D GetTransform3D(IDictionary source)
        {
            if(Helper != null)
            {
                return Helper.Create(typeof(Transform3D), source) as Transform3D;
            }
            return null;
        }

    }
}
