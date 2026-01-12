using System;

namespace System.Windows.Media
{
#if !IS_WINDOWS
    /// <summary>
    /// Abstract class that provides object representation of an image.
    /// </summary>
    public abstract class ImageSource : IFormattable
    {
        /// <summary>
        /// Gets the width of the image in measure units (96ths of an inch).
        /// </summary>
        public abstract double Width { get; }

        /// <summary>
        /// Gets the height of the image in measure units (96ths of an inch).
        /// </summary>
        public abstract double Height { get; }

        /// <summary>
        /// Gets the metadata associated with this image source.
        /// </summary>
        public virtual ImageMetadata? Metadata => null;

        /// <summary>
        /// Returns a string representation of this ImageSource.
        /// </summary>
        /// <returns>A string representation of this ImageSource.</returns>
        public override string ToString()
        {
            return ToString(null, null);
        }

        /// <summary>
        /// Returns a string representation of this ImageSource using the specified format.
        /// </summary>
        /// <param name="formatProvider">The format provider to use.</param>
        /// <returns>A string representation of this ImageSource.</returns>
        public string ToString(IFormatProvider? formatProvider)
        {
            return ToString(null, formatProvider);
        }

        /// <summary>
        /// Returns a string representation of this ImageSource using the specified format and format provider.
        /// </summary>
        /// <param name="format">The format to use.</param>
        /// <param name="formatProvider">The format provider to use.</param>
        /// <returns>A string representation of this ImageSource.</returns>
        public virtual string ToString(string? format, IFormatProvider? formatProvider)
        {
            return $"{GetType().Name} (Width={Width}, Height={Height})";
        }
    }

    /// <summary>
    /// Base class for image metadata.
    /// </summary>
    public abstract class ImageMetadata
    {
    }
#endif
}

