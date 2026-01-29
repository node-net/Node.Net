namespace System.Windows.Media
{
#if !IS_WINDOWS && USE_POLYFILL
    /// <summary>
    /// Implements a set of predefined colors.
    /// </summary>
    public static class Colors
    {
        /// <summary>
        /// Gets the system-defined color that has an ARGB value of #FF000000.
        /// </summary>
        public static Color Black => Color.FromRgb(0, 0, 0);

        /// <summary>
        /// Gets the system-defined color that has an ARGB value of #FFFFFFFF.
        /// </summary>
        public static Color White => Color.FromRgb(255, 255, 255);

        /// <summary>
        /// Gets the system-defined color that has an ARGB value of #FFFF0000.
        /// </summary>
        public static Color Red => Color.FromRgb(255, 0, 0);

        /// <summary>
        /// Gets the system-defined color that has an ARGB value of #FF00FF00.
        /// </summary>
        public static Color Green => Color.FromRgb(0, 255, 0);

        /// <summary>
        /// Gets the system-defined color that has an ARGB value of #FF0000FF.
        /// </summary>
        public static Color Blue => Color.FromRgb(0, 0, 255);

        /// <summary>
        /// Gets the system-defined color that has an ARGB value of #FFFFFF00.
        /// </summary>
        public static Color Yellow => Color.FromRgb(255, 255, 0);

        /// <summary>
        /// Gets the system-defined color that has an ARGB value of #FFFF00FF.
        /// </summary>
        public static Color Magenta => Color.FromRgb(255, 0, 255);

        /// <summary>
        /// Gets the system-defined color that has an ARGB value of #FF00FFFF.
        /// </summary>
        public static Color Cyan => Color.FromRgb(0, 255, 255);

        /// <summary>
        /// Gets the system-defined color that has an ARGB value of #FF808080.
        /// </summary>
        public static Color Gray => Color.FromRgb(128, 128, 128);

        /// <summary>
        /// Gets the system-defined color that has an ARGB value of #FFC0C0C0.
        /// </summary>
        public static Color Silver => Color.FromRgb(192, 192, 192);

        /// <summary>
        /// Gets the system-defined color that has an ARGB value of #FF800000.
        /// </summary>
        public static Color Maroon => Color.FromRgb(128, 0, 0);

        /// <summary>
        /// Gets the system-defined color that has an ARGB value of #FF008000.
        /// </summary>
        public static Color Olive => Color.FromRgb(128, 128, 0);

        /// <summary>
        /// Gets the system-defined color that has an ARGB value of #FF000080.
        /// </summary>
        public static Color Navy => Color.FromRgb(0, 0, 128);

        /// <summary>
        /// Gets the system-defined color that has an ARGB value of #FF800080.
        /// </summary>
        public static Color Purple => Color.FromRgb(128, 0, 128);

        /// <summary>
        /// Gets the system-defined color that has an ARGB value of #FF008080.
        /// </summary>
        public static Color Teal => Color.FromRgb(0, 128, 128);

        /// <summary>
        /// Gets the system-defined color that has an ARGB value of #FFFFA500.
        /// </summary>
        public static Color Orange => Color.FromRgb(255, 165, 0);

        /// <summary>
        /// Gets the system-defined color that has an ARGB value of #FFFFC0CB.
        /// </summary>
        public static Color Pink => Color.FromRgb(255, 192, 203);

        /// <summary>
        /// Gets the system-defined color that has an ARGB value of #FFA52A2A.
        /// </summary>
        public static Color Brown => Color.FromRgb(165, 42, 42);

        /// <summary>
        /// Gets the system-defined color that has an ARGB value of #FF000000 with alpha 0 (transparent).
        /// </summary>
        public static Color Transparent => Color.FromArgb(0, 0, 0, 0);
    }
#endif
}

