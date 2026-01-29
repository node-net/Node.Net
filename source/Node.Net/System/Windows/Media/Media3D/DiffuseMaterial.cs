using System.Windows.Media;

namespace System.Windows.Media.Media3D
{
#if !IS_WINDOWS && USE_POLYFILL
    /// <summary>
    /// Allows the application of a 2-D brush, like a SolidColorBrush or TileBrush, to a diffusely lit 3-D model.
    /// </summary>
    public class DiffuseMaterial : Material
    {
        private Brush? _brush;

        /// <summary>
        /// Gets or sets the Brush to be applied as a Material to a 3-D model.
        /// </summary>
        public Brush? Brush
        {
            get => _brush;
            set => _brush = value;
        }

        /// <summary>
        /// Initializes a new instance of the DiffuseMaterial class.
        /// </summary>
        public DiffuseMaterial()
        {
        }

        /// <summary>
        /// Initializes a new instance of the DiffuseMaterial class with the specified Brush.
        /// </summary>
        /// <param name="brush">The Brush to be applied to the material.</param>
        public DiffuseMaterial(Brush brush)
        {
            _brush = brush;
        }
    }
#endif
}

