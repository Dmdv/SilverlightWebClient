using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Data;
using WebClient.ICS.Client.Model;

namespace WebClient.ICS.Client.Converters
{
	/// <summary>
	/// 	Конвертер string to DataType. Используется при хранении текущего DataTypeEnum.
	/// </summary>
	public class StringToDataTypeConverter : IValueConverter
	{
		/// <summary>
		/// 	Принимает string и возвращает DataType.
		/// </summary>
		/// <returns> Значение, передаваемое целевому свойству зависимостей. </returns>
		/// <param name="value"> Исходные данные, передаваемые целевому объекту. </param>
		/// <param name="targetType"> Тип <see cref="T:System.Type" /> 
		/// данных, ожидаемый целевым свойством зависимостей. </param>
		/// <param name="parameter"> Необязательный параметр для использования в логике преобразователя. </param>
		/// <param name="culture"> Язык и региональные параметры преобразования. </param>
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			Debug.WriteLine("StringToDataTypeConverter");

			return value == null
				? DataTypeEnum.String
				: Enum.Parse(typeof (DataTypeEnum), (string) value, true);
		}

		/// <summary>
		/// 	Not implemented.
		/// </summary>
		/// <param name="value"> </param>
		/// <param name="targetType"> </param>
		/// <param name="parameter"> </param>
		/// <param name="culture"> </param>
		/// <returns> </returns>
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}