﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Node.Net.Internal
{
	sealed class Visual3DFactory : IFactory
	{
		public object Create(Type targetType, object source)
		{
			if (source == null) return null;
			if (targetType == null) return null;
			if (ParentFactory != null)
			{
				var model3D = ParentFactory.Create<Model3D>(source);
				if (model3D != null) return new ModelVisual3D { Content = model3D };
			}
			return null;
		}
		public IFactory ParentFactory { get; set; }
	}
}
