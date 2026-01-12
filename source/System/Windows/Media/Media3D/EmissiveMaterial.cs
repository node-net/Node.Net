using System.Windows.Media;

namespace System.Windows.Media.Media3D
{
#if !IS_WINDOWS
    /// <summary>
    /// Allows the application of a Brush to a 3-D model so that it participates in lighting calculations as if the Brush were emitting light.
    /// </summary>
    public class EmissiveMaterial : Material
    {
        private Brush? _brush;

        /// <summary>
        /// Gets or sets the Brush applied to the EmissiveMaterial.
        /// </summary>
        public Brush? Brush
        {
            get => _brush;
            set => _brush = value;
        }

        /// <summary>
        /// Initializes a new instance of the EmissiveMaterial class.
        /// </summary>
        public EmissiveMaterial()
        {
        }

        /// <summary>
        /// Initializes a new instance of the EmissiveMaterial class with the specified Brush.
        /// </summary>
        /// <param name="brush">The Brush to be applied to the material.</param>
        public EmissiveMaterial(Brush brush)
        {
            _brush = brush;
        }
    }
#endif
}

