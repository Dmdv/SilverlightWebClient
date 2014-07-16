using System;

namespace WebClient.ICS.Client.Model
{
    /// <summary>
    /// Validates values.
    /// </summary>
    public class ClientValidator
    {
        private readonly DataTypeEnum? _dataType;

        /// <summary>
        /// Создает валидатор типа.
        /// </summary>
        /// <param name="dataType"></param>
        private ClientValidator(DataTypeEnum? dataType)
        {
            _dataType = dataType;
        }

        /// <summary>
        /// Возвращает валидатор типа.
        /// </summary>
        /// <param name="dataType"></param>
        /// <returns></returns>
        public static ClientValidator ForType(DataTypeEnum? dataType)
        {
            return new ClientValidator(dataType);
        }

        /// <summary>
        /// Validates value.
        /// </summary>
        /// <param name="minimumValue">В случае строки stringValue = это xml файл или строковое выражение.</param>
        /// <param name="maximumValue">Верхняя граница для числовых значений.</param>
        /// <param name="data">Данные. Значение поля Data.</param>
        /// <exception cref="ArgumentException"></exception>
        public void Validate(string minimumValue, string maximumValue, object data)
        {
            var isMaxEmpty = string.IsNullOrEmpty(maximumValue);
            var isMinEmpty = string.IsNullOrEmpty(minimumValue);

            switch (_dataType)
            {
                case DataTypeEnum.Bin:
                case DataTypeEnum.Xml:
                case DataTypeEnum.String:
                    if (isMinEmpty)
                    {
                        break;
                    }

                    ValidateString(minimumValue, data);

                    break;

                case DataTypeEnum.Long:
                    var longMax = 0L;
                    var longMin = 0L;
                    var skip = isMaxEmpty || isMinEmpty;

                    if (!isMaxEmpty) longMax = GetLong(maximumValue);
                    if (!isMinEmpty) longMin = GetLong(minimumValue);

                    if (!skip && longMin >= longMax)
                    {
                        throw new ArgumentException(string.Format("Validation error: minimum exceeds maximum"));
                    }

                    var longValue = data.Cast<long>();

                    if (!isMaxEmpty && longValue > longMax)
                    {
                        var message = string.Format("Validation error: Data exceeds Maximum: '{0}'", longMax);
                        throw new ArgumentException(message);
                    }

                    if (!isMinEmpty && longValue < longMin)
                    {
                        var message = string.Format("Validation error: Data is less than Minimum: '{0}'", longMin);
                        throw new ArgumentException(message);
                    }

                    break;

                case DataTypeEnum.Double:
                    var doubleMax = 0D;
                    var doubleMin = 0D;
                    var skip2 = isMaxEmpty || isMinEmpty;

                    if (!isMaxEmpty) doubleMax = GetDouble(maximumValue);
                    if (!isMinEmpty) doubleMin = GetDouble(minimumValue);

                    if (!skip2 && doubleMin >= doubleMax)
                    {
                        throw new ArgumentException(string.Format("Validation error: minimum exceeds maximum"));
                    }

                    if (!isMaxEmpty && data.Cast<double>() > doubleMax)
                    {
                        var message = string.Format("Validation error: Data exceeds Maximum: '{0}' ", doubleMax);
                        throw new ArgumentException(message);
                    }

                    if (!isMinEmpty && data.Cast<double>() < doubleMin)
                    {
                        var message = string.Format("Validation error: Data is less than Minimum: '{0}'", doubleMin);
                        throw new ArgumentException(message);
                    }

                    break;

                case DataTypeEnum.DateTime:
                    if (isMinEmpty) break;

                    DateTime dt;
                    var tryParse = DateTime.TryParse(minimumValue, out dt);
                    if (!tryParse)
                    {
                        var message = string.Format("Validation error: value is not valid: '{0}' ", minimumValue);
                        throw new ArgumentException(message);
                    }

                    break;
                default:
                    break;
            }
        }

        private static void ValidateString(string minimumValue, object data)
        {
            var longlength = GetLong(minimumValue);

            if (longlength < 0)
            {
                throw new ArgumentException(string.Format("Value must be >= 0: '{0}'", longlength));
            }

            long length = data is string ? data.Cast<string>().Length : data.Cast<byte[]>().Length;

            if (length > longlength)
            {
                var message = string.Format("Length value exceeds string length: '{0}'", longlength);
                throw new ArgumentException(message);
            }
        }

        private static double GetDouble(string value)
        {
            double doubleValue;
            var tryParse = double.TryParse(value, out doubleValue);
            if (!tryParse)
            {
                throw new ArgumentException(string.Format("Value is not valid: '{0}'", value));
            }
            return doubleValue;
        }

        private static long GetLong(string value)
        {
            long longMin;
            var tryParse = long.TryParse(value, out longMin);
            if (!tryParse)
            {
                throw new ArgumentException(string.Format("Value is not valid: '{0}'", value));
            }
            return longMin;
        }
    }
}
