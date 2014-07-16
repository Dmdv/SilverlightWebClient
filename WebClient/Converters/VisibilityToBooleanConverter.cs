using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using WebClient.ICS.Client.Model;

namespace WebClient.ICS.Client.Converters
{
    /// <summary>
    /// Преобразует Visibility в Boolean.
    /// </summary>
    public class VisibilityToBooleanConverter : IValueConverter
    {
        /// <summary>
        /// Сконвертировать из Visibility в Boolean.
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return false;
            return value.Cast<Visibility>() == Visibility.Visible;
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