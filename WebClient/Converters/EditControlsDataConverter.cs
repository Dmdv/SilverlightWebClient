using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Data;
using WebClient.ICS.Client.Model;

namespace WebClient.ICS.Client.Converters
{
    /// <summary>
    /// Используется при присвоении значений в контролы для редактирования данных настройки.
    /// </summary>
    public class EditControlsDataConverter : IValueConverter
    {
        const NumberStyles Styles = NumberStyles.Integer | NumberStyles.AllowDecimalPoint;

        /// <summary>
        /// Вызывается при присвоении значения в контрол.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Debug.WriteLine(string.Format(">>> EditControlsDataConverter [Convert]: value = '{0}', parameter = '{1}'", value, parameter));

            if (value != null)
            {
                if ((string)parameter == "NumbersEditor" && !((value is long) || (value is double) || (value is int)))
                {
                    return "0";
                }

                if (value is string || value is bool)
                {
                    return value.ToString();
                }
            }
            else
            {
                return string.Empty;
            }

            return value;
        }

        /// <summary>
        /// Вызывается при сохранении данных из контрола в целевое свойство бизнес-объекта.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Debug.WriteLine(string.Format(">>> EditControlsDataConverter [ConvertBack]: value = '{0}', parameter = '{1}'", value, parameter));

            var resultValue = value;

            if (value is string)
            {
                var str = value.Cast<string>();
                var dataTypeStatic = CurrentDataTypeInfo.DataTypeStatic;

                try
                {
                    switch (dataTypeStatic)
                    {
                        case DataTypeEnum.Bool:
                            bool result;
                            resultValue = Boolean.TryParse(str, out result) && result;
                            break;
                        case DataTypeEnum.Long:
                            if (str == string.Empty) str = "0";
                            resultValue = long.Parse(str);
                            break;
                        case DataTypeEnum.Double:
                            if (str == string.Empty) str = "0";
                            resultValue = Double.Parse(str, Styles, NumberFormatInfo.InvariantInfo);
                            break;
                        case DataTypeEnum.String:
                            resultValue = str;
                            break;
                    }
                }
                catch (Exception exception)
                {
                    resultValue = new ArgumentException("Input value is not valid: " + str, exception);
                }
            }

            Debug.WriteLine(string.Format(">>> EditControlsDataConverter [LAST DATA] = {0}", resultValue));
            return resultValue;
        }
    }
}
