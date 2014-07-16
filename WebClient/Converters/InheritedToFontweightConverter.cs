using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using WebClient.ICS.Client.Model;

namespace WebClient.ICS.Client.Converters
{
    /// <summary>
    /// Используется для выделения наследуемых прав.
    /// </summary>
    public class InheritedToFontweightConverter : IValueConverter
    {
        /// <summary>
        /// Преобразовать bool к типу шрифта для выделения.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null
                       ? FontWeights.Normal
                       : (value.Cast<bool>() ? FontWeights.Normal : FontWeights.SemiBold);
        }

        /// <summary>
        /// Не используется.
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
