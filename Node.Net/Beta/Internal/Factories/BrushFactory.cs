using System;
using System.Windows.Media;

namespace Node.Net.Beta.Internal.Factories
{
    sealed class BrushFactory : Node.Net.Beta.IFactory
    {
        public object Create(Type target_type, object source)
        {
            if (source != null)
            {
                if (source.GetType() == typeof(Color)) return new SolidColorBrush((Color)source);
                if (source.GetType() == typeof(ImageSource)) return CreateFromImageSource(source as ImageSource);
            }
            if (ParentFactory != null)
            {
                var color = ParentFactory.Create(typeof(Color), source);
                if (color != null) return Create(target_type, color);
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
