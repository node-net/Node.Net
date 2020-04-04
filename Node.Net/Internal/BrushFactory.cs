using System;
using System.Windows.Media;

namespace Node.Net.Internal
{
    internal sealed class BrushFactory : IFactory
    {
        public object Create(Type targetType, object source)
        {
            if (source != null)
            {
                if (source is Color)
                {
                    return new SolidColorBrush((Color)source);
                }

                if (source is ImageSource)
                {
                    return CreateFromImageSource(source as ImageSource);
                }
            }
            if (ParentFactory != null)
            {
                object? color = ParentFactory.Create(typeof(Color), source);
                if (color != null)
                {
                    return Create(targetType, color);
                }

                object? image = ParentFactory.Create(typeof(ImageSource), source);
                if (image != null)
                {
                    return Create(targetType, image);
                }
            }
#pragma warning disable CS8603 // Possible null reference return.
            return null;
#pragma warning restore CS8603 // Possible null reference return.
        }

        public IFactory ParentFactory { get; set; }

        private static Brush CreateFromImageSource(ImageSource source)
        {
            if (source != null)
            {
                return new ImageBrush { ImageSource = source, TileMode = TileMode.Tile };
            }
#pragma warning disable CS8603 // Possible null reference return.
            return null;
#pragma warning restore CS8603 // Possible null reference return.
        }
    }
}