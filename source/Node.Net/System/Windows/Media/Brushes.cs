namespace System.Windows.Media
{
#if !IS_WINDOWS && USE_POLYFILL
    /// <summary>
    /// Implements a set of predefined SolidColorBrush objects.
    /// </summary>
    public static class Brushes
    {
        /// <summary>
        /// Gets the system-defined SolidColorBrush that has a hexadecimal value of #FF000000.
        /// </summary>
        public static SolidColorBrush Black => new SolidColorBrush(Colors.Black);

        /// <summary>
        /// Gets the system-defined SolidColorBrush that has a hexadecimal value of #FFFFFFFF.
        /// </summary>
        public static SolidColorBrush White => new SolidColorBrush(Colors.White);

        /// <summary>
        /// Gets the system-defined SolidColorBrush that has a hexadecimal value of #FFFF0000.
        /// </summary>
        public static SolidColorBrush Red => new SolidColorBrush(Colors.Red);

        /// <summary>
        /// Gets the system-defined SolidColorBrush that has a hexadecimal value of #FF00FF00.
        /// </summary>
        public static SolidColorBrush Green => new SolidColorBrush(Colors.Green);

        /// <summary>
        /// Gets the system-defined SolidColorBrush that has a hexadecimal value of #FF0000FF.
        /// </summary>
        public static SolidColorBrush Blue => new SolidColorBrush(Colors.Blue);

        /// <summary>
        /// Gets the system-defined SolidColorBrush that has a hexadecimal value of #FFFFFF00.
        /// </summary>
        public static SolidColorBrush Yellow => new SolidColorBrush(Colors.Yellow);

        /// <summary>
        /// Gets the system-defined SolidColorBrush that has a hexadecimal value of #FFFF00FF.
        /// </summary>
        public static SolidColorBrush Magenta => new SolidColorBrush(Colors.Magenta);

        /// <summary>
        /// Gets the system-defined SolidColorBrush that has a hexadecimal value of #FF00FFFF.
        /// </summary>
        public static SolidColorBrush Cyan => new SolidColorBrush(Colors.Cyan);

        /// <summary>
        /// Gets the system-defined SolidColorBrush that has a hexadecimal value of #FF808080.
        /// </summary>
        public static SolidColorBrush Gray => new SolidColorBrush(Colors.Gray);

        /// <summary>
        /// Gets the system-defined SolidColorBrush that has a hexadecimal value of #FFC0C0C0.
        /// </summary>
        public static SolidColorBrush Silver => new SolidColorBrush(Colors.Silver);

        /// <summary>
        /// Gets the system-defined SolidColorBrush that has a hexadecimal value of #FF800000.
        /// </summary>
        public static SolidColorBrush Maroon => new SolidColorBrush(Colors.Maroon);

        /// <summary>
        /// Gets the system-defined SolidColorBrush that has a hexadecimal value of #FF008000.
        /// </summary>
        public static SolidColorBrush Olive => new SolidColorBrush(Colors.Olive);

        /// <summary>
        /// Gets the system-defined SolidColorBrush that has a hexadecimal value of #FF000080.
        /// </summary>
        public static SolidColorBrush Navy => new SolidColorBrush(Colors.Navy);

        /// <summary>
        /// Gets the system-defined SolidColorBrush that has a hexadecimal value of #FF800080.
        /// </summary>
        public static SolidColorBrush Purple => new SolidColorBrush(Colors.Purple);

        /// <summary>
        /// Gets the system-defined SolidColorBrush that has a hexadecimal value of #FF008080.
        /// </summary>
        public static SolidColorBrush Teal => new SolidColorBrush(Colors.Teal);

        /// <summary>
        /// Gets the system-defined SolidColorBrush that has a hexadecimal value of #FFFFA500.
        /// </summary>
        public static SolidColorBrush Orange => new SolidColorBrush(Colors.Orange);

        /// <summary>
        /// Gets the system-defined SolidColorBrush that has a hexadecimal value of #FFFFC0CB.
        /// </summary>
        public static SolidColorBrush Pink => new SolidColorBrush(Colors.Pink);

        /// <summary>
        /// Gets the system-defined SolidColorBrush that has a hexadecimal value of #FFA52A2A.
        /// </summary>
        public static SolidColorBrush Brown => new SolidColorBrush(Colors.Brown);

        /// <summary>
        /// Gets the system-defined SolidColorBrush that has a hexadecimal value of #00000000 (transparent).
        /// </summary>
        public static SolidColorBrush Transparent => new SolidColorBrush(Colors.Transparent);
    }
#endif
}

