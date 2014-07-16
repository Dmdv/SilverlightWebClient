using System;
using System.Globalization;
using System.Windows.Data;

namespace WebClient.ICS.Client.Converters
{
    /// <summary>
    /// Используется, чтобы disable элемент, если его объект привязки не способен вернуть значение.
    /// </summary>
    public class NullToEnabledConverter : IValueConverter
    {
        /// <summary>
        /// Возвращает false, если DataSource = null.
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? false : true;
        }

        /// <summary>
        /// Не используется.
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}