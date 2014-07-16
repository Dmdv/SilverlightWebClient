using System;
using System.Globalization;
using WebClient.ICS.Client.Commands;
using WebClient.ICS.Client.ViewModel;

namespace WebClient.ICS.Client.Converters
{
    /// <summary>
    /// Класс параметров для передачи значений в команды.
    /// </summary>
    public class NewNodeParameterConverter : IMultiValueConverter
    {
        /// <summary>
        /// Convert
        /// </summary>
        /// <param name="values"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return new CommandNodeParameter
                       {
                           NodeName = (string) values[1],
                           SettingNode = (SettingNodeViewModel)values[0]
                       };
        }

        /// <summary>
        /// ConvertBack
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetTypes"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}