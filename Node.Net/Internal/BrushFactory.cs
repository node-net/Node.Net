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
#pragma warning disable CS8604 // Possible null reference argument.
                    return CreateFromImageSource(source as ImageSource);
#pragma warning restore CS8604 // Possible null reference argument.
                }
            }
            if (ParentFactory != null)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                object? color = ParentFactory.Create(typeof(Color), source);
#pragma warning restore CS8604 // Possible null reference argument.
                if (color != null)
                {
                    return Create(targetType, color);
                }

#pragma warning disable CS8604 // Possible null reference argument.
                object? image = ParentFactory.Create(typeof(ImageSource), source);
#pragma warning restore CS8604 // Possible null reference argument.
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