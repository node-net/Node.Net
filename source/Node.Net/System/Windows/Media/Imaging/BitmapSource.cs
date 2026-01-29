using System;
using System.Windows.Media;

namespace System.Windows.Media.Imaging
{
#if !IS_WINDOWS && USE_POLYFILL
    /// <summary>
    /// Represents a single, constant set of pixels at a certain size and resolution.
    /// </summary>
    public abstract class BitmapSource : ImageSource
    {
        private int _pixelWidth;
        private int _pixelHeight;
        private double _dpiX = 96.0;
        private double _dpiY = 96.0;

        /// <summary>
        /// Gets the width of the bitmap in pixels.
        /// </summary>
        public int PixelWidth
        {
            get => _pixelWidth;
            protected set => _pixelWidth = value;
        }

        /// <summary>
        /// Gets the height of the bitmap in pixels.
        /// </summary>
        public int PixelHeight
        {
            get => _pixelHeight;
            protected set => _pixelHeight = value;
        }

        /// <summary>
        /// Gets the horizontal dots per inch (dpi) of the bitmap.
        /// </summary>
        public double DpiX
        {
            get => _dpiX;
            protected set => _dpiX = value;
        }

        /// <summary>
        /// Gets the vertical dots per inch (dpi) of the bitmap.
        /// </summary>
        public double DpiY
        {
            get => _dpiY;
            protected set => _dpiY = value;
        }

        /// <summary>
        /// Gets the width of the image in measure units (96ths of an inch).
        /// </summary>
        public override double Width => PixelWidth * 96.0 / DpiX;

        /// <summary>
        /// Gets the height of the image in measure units (96ths of an inch).
        /// </summary>
        public override double Height => PixelHeight * 96.0 / DpiY;

        /// <summary>
        /// Creates a modifiable copy of this BitmapSource.
        /// </summary>
        /// <returns>A modifiable copy of this BitmapSource.</returns>
        public virtual BitmapSource Clone()
        {
            // For non-Windows, return a simple clone reference
            // In a real implementation, this would copy the pixel data
            return this;
        }
    }
#endif
}

