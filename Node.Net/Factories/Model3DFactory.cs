﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Media.Media3D;

namespace Node.Net.Factories
{
    public sealed class Model3DFactory : Generic.TargetTypeFactory<Model3D>
    {
        public Func<object,Model3D> PrimaryModel3DHelperFunction { get; set; }
        public override Model3D Create(object source)
        {
            if (source != null)
            {
                /*
                if(typeof(Node.Net.Factories.IElement).IsAssignableFrom(source.GetType()))
                {
                    var instance = CreateFromIElement(source as Node.Net.Factories.IElement);
                    if (instance != null) return instance;
                }
                */
                if(typeof(IDictionary).IsAssignableFrom(source.GetType()))
                {
                    var instance = CreateFromDictionary(source as IDictionary);
                    if (instance != null) return instance;
                }
            }

            if (Helper != null)
            {
                var geometry3D = Helper.Create(typeof(GeometryModel3D), source) as GeometryModel3D;
                if (geometry3D != null) return geometry3D;
            }
            return null;
        }

        private bool cache = false;
        private Dictionary<object, Model3D> model3DCache = new Dictionary<object, Model3D>();
        public bool Cache
        {
            get { return cache; }
            set
            {
                if(cache != value)
                {
                    cache = value;
                    model3DCache.Clear();
                }
            }
        }
        private Model3D CreateFromDictionary(IDictionary source)
        {
            if(cache)
            {
                if (model3DCache.ContainsKey(source)) return model3DCache[source];
            }
            var model3DGroup = new Model3DGroup { Transform = GetTransform3D(source) };
            var primaryModel = GetPrimaryModel3D(source);
            if (primaryModel != null) model3DGroup.Children.Add(primaryModel);
            
            foreach(var key in source.Keys)
            {
                var child_dictionary = source[key] as IDictionary;
                if(child_dictionary != null)
                {
                    var child_model = Create(child_dictionary);
                    if (child_model != null) model3DGroup.Children.Add(child_model);
                }
            }
            if (model3DGroup.Children.Count > 0)
            {
                if (cache) model3DCache.Add(source, model3DGroup);
                return model3DGroup;
            }
            return null;
        }
        /*
        private Model3D CreateFromIElement(Node.Net.Factories.IElement source)
        {
            if (cache)
            {
                if (model3DCache.ContainsKey(source)) return model3DCache[source];
            }
            var model3DGroup = new Model3DGroup { Transform = GetTransform3D(source) };
            var primaryModel = GetPrimaryModel3D(source);
            if (primaryModel != null) model3DGroup.Children.Add(primaryModel);

            foreach (var key in source.Keys)
            {
                var child_dictionary = source.Get(key);// as IDictionary;
                if (child_dictionary != null)
                {
                    var child_model = Create(child_dictionary);
                    if (child_model != null) model3DGroup.Children.Add(child_model);
                }
            }
            if (model3DGroup.Children.Count > 0)
            {
                if (cache) model3DCache.Add(source, model3DGroup);
                return model3DGroup;
            }
            return null;
        }*/

        private Model3D GetPrimaryModel3D(object source)
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
        private Transform3D GetTransform3D(object source)
        {
            if(Helper != null)
            {
                return Helper.Create(typeof(Transform3D), source) as Transform3D;
            }
            return null;
        }

    }
}
