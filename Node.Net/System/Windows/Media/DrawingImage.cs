using System;
using System.Windows.Media;

namespace System.Windows.Media
{
#if !IS_WINDOWS
    /// <summary>
    /// An ImageSource that uses a Drawing as its content.
    /// </summary>
    public class DrawingImage : ImageSource
    {
        private Drawing? _drawing;

        /// <summary>
        /// Gets or sets the Drawing that defines the content of this DrawingImage.
        /// </summary>
        public Drawing? Drawing
        {
            get => _drawing;
            set => _drawing = value;
        }

        /// <summary>
        /// Initializes a new instance of the DrawingImage class.
        /// </summary>
        public DrawingImage()
        {
        }

        /// <summary>
        /// Initializes a new instance of the DrawingImage class with the specified Drawing.
        /// </summary>
        /// <param name="drawing">The Drawing that defines the content of this DrawingImage.</param>
        public DrawingImage(Drawing drawing)
        {
            _drawing = drawing;
        }

        /// <summary>
        /// Gets the width of the image in measure units (96ths of an inch).
        /// </summary>
        public override double Width
        {
            get
            {
                if (_drawing != null && _drawing is GeometryDrawing geometryDrawing)
                {
                    return geometryDrawing.Geometry?.Bounds.Width ?? 0;
                }
                return 0;
            }
        }

        /// <summary>
        /// Gets the height of the image in measure units (96ths of an inch).
        /// </summary>
        public override double Height
        {
            get
            {
                if (_drawing != null && _drawing is GeometryDrawing geometryDrawing)
                {
                    return geometryDrawing.Geometry?.Bounds.Height ?? 0;
                }
                return 0;
            }
        }
    }

    /// <summary>
    /// Base class for objects that describe how to draw graphics.
    /// </summary>
    public abstract class Drawing
    {
    }

    /// <summary>
    /// Defines objects used to paint graphical objects.
    /// </summary>
    public abstract class Brush
    {
    }

    /// <summary>
    /// Paints an area with a solid color.
    /// </summary>
    public class SolidColorBrush : Brush
    {
        private Color _color;

        /// <summary>
        /// Gets or sets the color of this SolidColorBrush.
        /// </summary>
        public Color Color
        {
            get => _color;
            set => _color = value;
        }

        /// <summary>
        /// Initializes a new instance of the SolidColorBrush class.
        /// </summary>
        public SolidColorBrush()
        {
            _color = Colors.Black;
        }

        /// <summary>
        /// Initializes a new instance of the SolidColorBrush class with the specified Color.
        /// </summary>
        /// <param name="color">The color to apply to the brush.</param>
        public SolidColorBrush(Color color)
        {
            _color = color;
        }
    }

    /// <summary>
    /// Draws a Geometry using the specified Brush and Pen.
    /// </summary>
    public class GeometryDrawing : Drawing
    {
        private Geometry? _geometry;
        private Brush? _brush;
        private Pen? _pen;

        /// <summary>
        /// Gets or sets the Geometry that describes the shape of this GeometryDrawing.
        /// </summary>
        public Geometry? Geometry
        {
            get => _geometry;
            set => _geometry = value;
        }

        /// <summary>
        /// Gets or sets the Brush used to fill the interior of the shape described by this GeometryDrawing.
        /// </summary>
        public Brush? Brush
        {
            get => _brush;
            set => _brush = value;
        }

        /// <summary>
        /// Gets or sets the Pen used to stroke this GeometryDrawing.
        /// </summary>
        public Pen? Pen
        {
            get => _pen;
            set => _pen = value;
        }
    }

    /// <summary>
    /// Base class for objects that define geometric shapes.
    /// </summary>
    public abstract class Geometry
    {
        /// <summary>
        /// Gets a Rect that specifies the axis-aligned bounding box of the Geometry.
        /// </summary>
        public abstract Rect Bounds { get; }
    }

    /// <summary>
    /// Describes a 2-D rectangle.
    /// </summary>
    public struct Rect
    {
        private double _x;
        private double _y;
        private double _width;
        private double _height;

        /// <summary>
        /// Gets or sets the x-coordinate of the left side of the rectangle.
        /// </summary>
        public double X
        {
            get => _x;
            set => _x = value;
        }

        /// <summary>
        /// Gets or sets the y-coordinate of the top side of the rectangle.
        /// </summary>
        public double Y
        {
            get => _y;
            set => _y = value;
        }

        /// <summary>
        /// Gets or sets the width of the rectangle.
        /// </summary>
        public double Width
        {
            get => _width;
            set => _width = value;
        }

        /// <summary>
        /// Gets or sets the height of the rectangle.
        /// </summary>
        public double Height
        {
            get => _height;
            set => _height = value;
        }

        /// <summary>
        /// Initializes a new instance of the Rect structure.
        /// </summary>
        public Rect(double x, double y, double width, double height)
        {
            _x = x;
            _y = y;
            _width = width;
            _height = height;
        }
    }

    /// <summary>
    /// Describes how a shape is stroked.
    /// </summary>
    public class Pen
    {
        private Brush? _brush;
        private double _thickness = 1.0;

        /// <summary>
        /// Gets or sets the Brush used to draw the stroke.
        /// </summary>
        public Brush? Brush
        {
            get => _brush;
            set => _brush = value;
        }

        /// <summary>
        /// Gets or sets the thickness of the stroke.
        /// </summary>
        public double Thickness
        {
            get => _thickness;
            set => _thickness = value;
        }
    }
#endif
}

