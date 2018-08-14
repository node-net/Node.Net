﻿using System;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Node.Net.Internal
{
	internal class MaterialFactory
	{
		public object Create(Type targetType, object source)
		{
			if (targetType == null) return null;
			if (!typeof(Material).IsAssignableFrom(targetType)) return null;
			if (source != null)
			{
				if (typeof(Brush).IsAssignableFrom(source.GetType()))
				{
					return new DiffuseMaterial(source as Brush);
				}
			}
			if (ParentFactory != null)
			{
				var brush = ParentFactory.Create<Brush>(source);
				if (brush != null) return new DiffuseMaterial(brush);
			}
			return null;
		}

		public IFactory ParentFactory { get; set; }
	}
}