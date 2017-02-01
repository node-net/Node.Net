using System.Reflection;
using System.Windows.Media;

namespace Node.Net.Factories
{
    public sealed class BrushFactory : Generic.TargetTypeFactory<Brush>
    {
        public Brush DefaultBrush { get; set; }
        public override Brush Create(object source)
        {
            Brush result = null;
            if (source != null)
            {
                if (source.GetType() == typeof(string))
                {
                    result = CreateFromString(source.ToString());
                    if (result != null) return result;
                }
                if(typeof(ImageSource).IsAssignableFrom(source.GetType()))
                {
                    result = CreateFromImageSource(source as ImageSource);
                    if (result != null) return result;
                }
            }


            var instance = GetHelper().Create(typeof(Color), source);
            if(instance != null)
            {
                return new SolidColorBrush((Color)instance);
            }

            return DefaultBrush;
        }

        private IFactory GetHelper()
        {
            if (Helper != null) return Helper;
            return new ColorFactory();
        }
        private Brush CreateFromString(string name)
        {
            foreach (PropertyInfo property in typeof(Brushes).GetProperties(BindingFlags.Public | BindingFlags.Static))
            {
                if (property.Name == name)
                {
                    return (Brush)property.GetValue(null, null);
                }
            }
            return null;
        }

        private Brush CreateFromImageSource(ImageSource source)
        {
            if(source != null)
            {
                return new ImageBrush { ImageSource = source, TileMode = TileMode.Tile };
            }
            return null;
        }
    }
}
