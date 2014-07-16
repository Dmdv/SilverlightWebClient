using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Data;
using WebClient.ICS.Client.Model;

namespace WebClient.ICS.Client.Converters
{
    /// <summary>
    /// Конвертер данных для <see cref="CurrentDataTypeInfo"/>
    /// Используется при хранении текущего DataType.
    /// </summary>
    public class CurrentDataTypeConverter : IValueConverter
    {
        /// <summary>
        /// Вызывается при присвоении в <see cref="CurrentDataTypeInfo.DataType"/> DataType текущего DataContext.
        /// </summary>
        /// <param name="value">DataType.</param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns>Возвращает DataType. По умолчанию, если DataType равен null, присваивается DataTypeEnum.String.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Debug.WriteLine("CurrentDataTypeConverter");
            Debug.WriteLine("[SELECT NODE] DataType = " + value ?? "null");
            return value ?? DataTypeEnum.String;
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