using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Data;

namespace WebClient.ICS.Client.Converters
{
    /// <summary>
    /// Convert from Data field to byte[]
    /// </summary>
    public class BytesToDataConverter : IValueConverter
    {
        /// <summary>
        /// Convert from Data field to byte[].
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Debug.WriteLine(">>> [Converter: BytesToDataConverter]");

            if (typeof (IEnumerable<byte>).IsAssignableFrom(targetType))
            {
                if (value as IEnumerable<byte> != null)
                {
                    return value;
                }
            }

            return null;
        }

        /// <summary>
        /// Convert from byte[] field to Data.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
