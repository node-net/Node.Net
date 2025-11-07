#if IS_WINDOWS
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Node.Net.Converters
{
    /// <summary>
    /// Converts an object to Visibilty.Hidden when null, otherwise Visibility.Visible
    /// </summary>
    public class VisibleWhenNull : IValueConverter
    {
        public static VisibleWhenNull Default { get; } = new VisibleWhenNull();

        /// <summary>
        /// Convert
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? Visibility.Visible : Visibility.Hidden;
        }

        /// <summary>
        /// ConvertBack (not implemented)
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
#endif
