using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Node.Net.Resources
{
    public class MaterialResources : Resources
    {
        public IResources ImageResources = null;

        protected override object GetDynamicResource(string name)
        {
            if(IsKnownColor(name))
            {
                var material = new DiffuseMaterial(new SolidColorBrush((Color)ColorConverter.ConvertFromString(name)));
                Add(name, material);
                return material;
            }

            if(ImageResources != null)
            {
                var imageSource = ImageResources.GetResource(name) as ImageSource;
                if(imageSource != null)
                {
                    var material = Model3D.MaterialHelper.GetImageMaterial(imageSource);
                    Add(name, material);
                    return material;
                }
            }
            return base.GetDynamicResource(name);
        }

        private bool IsKnownColor(string name)
        {
            try
            {
                var color = (Color)ColorConverter.ConvertFromString(name);
                var dcolor = System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
                if (dcolor.Name == name) return true;
            }
            catch (System.FormatException) { }
            
            return false;
        }
    }
}
