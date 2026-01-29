using System.Collections.ObjectModel;
using System.Windows.Media;

namespace System.Windows.Media.Media3D
{
#if !IS_WINDOWS && USE_POLYFILL
    /// <summary>
    /// Represents a collection of Material objects.
    /// </summary>
    public class MaterialGroup : Material
    {
        private MaterialCollection _children;

        /// <summary>
        /// Gets or sets a collection of Material objects.
        /// </summary>
        public MaterialCollection Children
        {
            get
            {
                if (_children == null)
                {
                    _children = new MaterialCollection();
                }
                return _children;
            }
            set => _children = value ?? new MaterialCollection();
        }

        /// <summary>
        /// Initializes a new instance of the MaterialGroup class.
        /// </summary>
        public MaterialGroup()
        {
            _children = new MaterialCollection();
        }
    }

    /// <summary>
    /// Represents an ordered collection of Material objects.
    /// </summary>
    public class MaterialCollection : Collection<Material>
    {
    }
#endif
}

