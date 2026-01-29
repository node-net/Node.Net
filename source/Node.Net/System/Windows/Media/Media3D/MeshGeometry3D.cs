using System.Collections.ObjectModel;
using System.Windows;

#if !IS_WINDOWS && USE_POLYFILL
namespace System.Windows.Media.Media3D
{
    /// <summary>
    /// Represents a 3-D mesh geometry.
    /// </summary>
    public class MeshGeometry3D
    {
        private Point3DCollection? _positions;
        private Int32Collection? _triangleIndices;
        private Vector3DCollection? _normals;
        private PointCollection? _textureCoordinates;

        /// <summary>
        /// Gets or sets a collection of Point3D objects that describe the positions of the mesh vertices.
        /// </summary>
        public Point3DCollection Positions
        {
            get
            {
                if (_positions == null)
                {
                    _positions = new Point3DCollection();
                }
                return _positions;
            }
            set => _positions = value ?? new Point3DCollection();
        }

        /// <summary>
        /// Gets or sets a collection of triangle indices for the MeshGeometry3D.
        /// </summary>
        public Int32Collection TriangleIndices
        {
            get
            {
                if (_triangleIndices == null)
                {
                    _triangleIndices = new Int32Collection();
                }
                return _triangleIndices;
            }
            set => _triangleIndices = value ?? new Int32Collection();
        }

        /// <summary>
        /// Gets or sets a collection of normal vectors for the MeshGeometry3D.
        /// </summary>
        public Vector3DCollection Normals
        {
            get
            {
                if (_normals == null)
                {
                    _normals = new Vector3DCollection();
                }
                return _normals;
            }
            set => _normals = value ?? new Vector3DCollection();
        }

        /// <summary>
        /// Gets or sets a collection of Point objects that represent the texture coordinates of the MeshGeometry3D.
        /// </summary>
        public PointCollection TextureCoordinates
        {
            get
            {
                if (_textureCoordinates == null)
                {
                    _textureCoordinates = new PointCollection();
                }
                return _textureCoordinates;
            }
            set => _textureCoordinates = value ?? new PointCollection();
        }

        /// <summary>
        /// Initializes a new instance of the MeshGeometry3D class.
        /// </summary>
        public MeshGeometry3D()
        {
            _positions = new Point3DCollection();
            _triangleIndices = new Int32Collection();
            _normals = new Vector3DCollection();
            _textureCoordinates = new PointCollection();
        }
    }

    /// <summary>
    /// Represents a collection of Point3D objects.
    /// </summary>
    public class Point3DCollection : Collection<Point3D>
    {
    }

    /// <summary>
    /// Represents a collection of integer values.
    /// </summary>
    public class Int32Collection : Collection<int>
    {
    }

    /// <summary>
    /// Represents a collection of Vector3D objects.
    /// </summary>
    public class Vector3DCollection : Collection<Vector3D>
    {
    }

    /// <summary>
    /// Represents a collection of Point objects.
    /// </summary>
    public class PointCollection : Collection<Point>
    {
    }
}
#endif

