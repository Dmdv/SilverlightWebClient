using System;
using System.Globalization;
using System.Windows.Data;
using WebClient.ICS.Client.Model;

namespace WebClient.ICS.Client.Converters
{
    /// <summary>
    /// См. контрол IsSuccessfull.
    /// </summary>
    public class CheckBoxConverter : IValueConverter
    {
        /// <summary>
        /// Из строки со значениями  “true”, “false”, “any” в checkbox.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            var str = value.Cast<string>();
            if (string.IsNullOrEmpty(str)) return null;

            if (str == "true") return true;
            if (str == "false") return false;

            return null;
        }

        /// <summary>
        /// Из checkbox в строку.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return "any";

            var ret = value.Cast<bool?>();

            if (!ret.HasValue) return "any";
            return ret.Value ? "true" : "false";
        }
    }
}
