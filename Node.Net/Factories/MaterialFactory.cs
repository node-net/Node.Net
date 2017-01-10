using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Node.Net.Factories
{
    public class MaterialFactory : Generic.TargetTypeFactory<Material>
    {
        public Material DefaultMaterial { get; set; }
        public override Material Create(object source)
        {
            if (source == null) return DefaultMaterial;
            var material = CreateFromBrush(source as Brush);
            if (material != null) return material;
            /*
            if (source != null)
            {
                if (typeof(Brush).IsAssignableFrom(source.GetType()))
                {
                    return CreateFromBrush(source as Brush);
                }
            }*/
            
            //if(Helper != null)
            //{
                var instance = GetHelper().Create(typeof(Brush), source);
                if (instance != null)
                {
                    return CreateFromBrush(instance as Brush);
                }
            //}
            return DefaultMaterial;
        }

        private Material CreateFromBrush(Brush brush)
        {
            if (brush == null) return null;
            return new DiffuseMaterial(brush);
        }

        private IFactory GetHelper()
        {
            if (Helper != null) return Helper;
            return new BrushFactory();
        }
    }
}
