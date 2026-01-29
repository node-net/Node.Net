using System.Windows.Media;

#if !IS_WINDOWS && USE_POLYFILL
namespace System.Windows.Media.Media3D
{
    /// <summary>
    /// Allows the application of a Brush to a 3-D model with specular lighting calculations.
    /// </summary>
    public class SpecularMaterial : Material
    {
        private Brush? _brush;
        private double _specularPower = 20.0;

        /// <summary>
        /// Gets or sets the Brush to be applied as a Material to a 3-D model.
        /// </summary>
        public Brush? Brush
        {
            get => _brush;
            set => _brush = value;
        }

        /// <summary>
        /// Gets or sets the specular power for the material, which indicates the degree to which the material reflects light and creates highlights.
        /// </summary>
        public double SpecularPower
        {
            get => _specularPower;
            set => _specularPower = value;
        }

        /// <summary>
        /// Initializes a new instance of the SpecularMaterial class.
        /// </summary>
        public SpecularMaterial()
        {
        }

        /// <summary>
        /// Initializes a new instance of the SpecularMaterial class with the specified Brush.
        /// </summary>
        /// <param name="brush">The Brush to be applied to the material.</param>
        public SpecularMaterial(Brush brush)
        {
            _brush = brush;
        }

        /// <summary>
        /// Initializes a new instance of the SpecularMaterial class with the specified Brush and SpecularPower.
        /// </summary>
        /// <param name="brush">The Brush to be applied to the material.</param>
        /// <param name="specularPower">The specular power for the material.</param>
        public SpecularMaterial(Brush brush, double specularPower)
        {
            _brush = brush;
            _specularPower = specularPower;
        }
    }
}
#endif

