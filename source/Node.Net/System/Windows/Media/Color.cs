using System;

namespace System.Windows.Media
{
#if !IS_WINDOWS
    /// <summary>
    /// Describes a color in terms of alpha, red, green, and blue channels.
    /// </summary>
    public struct Color : IEquatable<Color>
    {
        private byte _a;
        private byte _r;
        private byte _g;
        private byte _b;

        /// <summary>
        /// Gets or sets the sRGB alpha channel value of the color.
        /// </summary>
        public byte A
        {
            get => _a;
            set => _a = value;
        }

        /// <summary>
        /// Gets or sets the sRGB red channel value of the color.
        /// </summary>
        public byte R
        {
            get => _r;
            set => _r = value;
        }

        /// <summary>
        /// Gets or sets the sRGB green channel value of the color.
        /// </summary>
        public byte G
        {
            get => _g;
            set => _g = value;
        }

        /// <summary>
        /// Gets or sets the sRGB blue channel value of the color.
        /// </summary>
        public byte B
        {
            get => _b;
            set => _b = value;
        }

        /// <summary>
        /// Creates a new Color structure by using the specified sRGB color channel values.
        /// </summary>
        /// <param name="r">The red channel, R, of the new color. Values must be between 0 and 255.</param>
        /// <param name="g">The green channel, G, of the new color. Values must be between 0 and 255.</param>
        /// <param name="b">The blue channel, B, of the new color. Values must be between 0 and 255.</param>
        /// <returns>A Color structure with the specified values and an alpha channel value of 255.</returns>
        public static Color FromRgb(byte r, byte g, byte b)
        {
            return new Color { A = 255, R = r, G = g, B = b };
        }

        /// <summary>
        /// Creates a new Color structure by using the specified sRGB alpha channel and color channel values.
        /// </summary>
        /// <param name="a">The alpha channel, A, of the new color. Values must be between 0 and 255.</param>
        /// <param name="r">The red channel, R, of the new color. Values must be between 0 and 255.</param>
        /// <param name="g">The green channel, G, of the new color. Values must be between 0 and 255.</param>
        /// <param name="b">The blue channel, B, of the new color. Values must be between 0 and 255.</param>
        /// <returns>A Color structure with the specified values.</returns>
        public static Color FromArgb(byte a, byte r, byte g, byte b)
        {
            return new Color { A = a, R = r, G = g, B = b };
        }

        /// <summary>
        /// Tests whether two Color structures are identical.
        /// </summary>
        /// <param name="color1">The first Color structure to compare.</param>
        /// <param name="color2">The second Color structure to compare.</param>
        /// <returns>true if color1 and color2 are identical; otherwise, false.</returns>
        public static bool operator ==(Color color1, Color color2)
        {
            return color1._a == color2._a && color1._r == color2._r && color1._g == color2._g && color1._b == color2._b;
        }

        /// <summary>
        /// Tests whether two Color structures are not identical.
        /// </summary>
        /// <param name="color1">The first Color structure to compare.</param>
        /// <param name="color2">The second Color structure to compare.</param>
        /// <returns>true if color1 and color2 are not equal; otherwise, false.</returns>
        public static bool operator !=(Color color1, Color color2)
        {
            return !(color1 == color2);
        }

        /// <summary>
        /// Tests whether the specified object is a Color structure and is equivalent to this Color.
        /// </summary>
        /// <param name="obj">The object to compare to this Color structure.</param>
        /// <returns>true if obj is a Color structure and is identical to the current Color structure; otherwise, false.</returns>
        public override bool Equals(object? obj)
        {
            return obj is Color color && Equals(color);
        }

        /// <summary>
        /// Tests whether the specified Color structure is equivalent to this Color.
        /// </summary>
        /// <param name="other">The Color structure to compare to this Color.</param>
        /// <returns>true if other is identical to this Color structure; otherwise, false.</returns>
        public bool Equals(Color other)
        {
            return _a == other._a && _r == other._r && _g == other._g && _b == other._b;
        }

        /// <summary>
        /// Gets a hash code for this Color structure.
        /// </summary>
        /// <returns>A hash code for this Color structure.</returns>
        public override int GetHashCode()
        {
#if NETSTANDARD2_0
            return ((((_a.GetHashCode() * 397) ^ _r.GetHashCode()) * 397) ^ _g.GetHashCode()) * 397 ^ _b.GetHashCode();
#else
            return HashCode.Combine(_a, _r, _g, _b);
#endif
        }

        /// <summary>
        /// Creates a string representation of the color using the ARGB channels.
        /// </summary>
        /// <returns>The string representation of the Color structure.</returns>
        public override string ToString()
        {
            return $"#{A:X2}{R:X2}{G:X2}{B:X2}";
        }
    }
#endif
}

