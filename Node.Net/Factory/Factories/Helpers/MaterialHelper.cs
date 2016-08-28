using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Node.Net.Factory.Factories.Helpers
{
    public static class MaterialHelper //:// IDictionaryHelper
    {
        //public static MaterialHelper Default { get; } = new MaterialHelper();
        public static Material FromString(string source,IFactory factory)
        {
            if(factory != null)
            {
                var icolor = factory.Create<IColor>(source);
                if (icolor != null) return new DiffuseMaterial { Brush = new SolidColorBrush(icolor.Color) };
            }
            //var icolor = Parent.Get
            //var color = colorFromString.Create<Color>(source);
            //return new DiffuseMaterial { Brush = new SolidColorBrush(color) };
            return null;
        }
    }
}
