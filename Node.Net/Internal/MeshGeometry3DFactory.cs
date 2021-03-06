﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Media.Media3D;

namespace Node.Net.Internal
{
    internal sealed class MeshGeometry3DFactory : IFactory
    {
        public object Create(Type targetType, object source)
        {
            if (source != null && source is IDictionary)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                return CreateFromDictionary(source as IDictionary);
#pragma warning restore CS8604 // Possible null reference argument.
            }
            if (ParentFactory != null && source != null)
            {
                return Create(targetType, ParentFactory.Create<IDictionary>(source));
            }
#pragma warning disable CS8603 // Possible null reference return.
            return null;
#pragma warning restore CS8603 // Possible null reference return.
        }

        public IFactory ParentFactory { get; set; }

        private readonly Dictionary<string, MeshGeometry3D> cache = new Dictionary<string, MeshGeometry3D>();

        private MeshGeometry3D CreateFromDictionary(IDictionary source)
        {
            if (ParentFactory != null && source != null)
            {
                string? type = source.Get<string>("Type");
                string? name = $"MeshGeometry3D.{type}.xaml";
                if (cache.ContainsKey(name))
                {
                    return cache[name];
                }

                MeshGeometry3D? mesh = ParentFactory.Create<MeshGeometry3D>(name);
                cache.Add(name, mesh);
                return mesh;
            }

#pragma warning disable CS8603 // Possible null reference return.
            return null;
#pragma warning restore CS8603 // Possible null reference return.
        }
    }
}