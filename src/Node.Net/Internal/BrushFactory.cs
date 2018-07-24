using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Node.Net.Internal
{
	internal sealed class BrushFactory : IFactory
	{
		public object Create(Type targetType, object source)
		{
			if (source != null)
			{
				if (source.GetType() == typeof(Color)) return new SolidColorBrush((Color)source);
				if (typeof(ImageSource).IsAssignableFrom(source.GetType())) return CreateFromImageSource(source as ImageSource);
			}
			if (ParentFactory != null)
			{
				var color = ParentFactory.Create(typeof(Color), source);
				if (color != null) return Create(targetType, color);

				var image = ParentFactory.Create(typeof(ImageSource), source);
				if (image != null) return Create(targetType, image);
			}
			return null;
		}

		public IFactory ParentFactory { get; set; }
		private static Brush CreateFromImageSource(ImageSource source)
		{
			if (source != null)
			{
				return new ImageBrush { ImageSource = source, TileMode = TileMode.Tile };
			}
			return null;
		}
	}
}
